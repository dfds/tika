import { parse, parseSideColumns } from "./../parser";
import {executeCli } from "./executeCli";
import { GetConfig } from "../../config";

export class CcloudApiKeys implements ApiKeys {
    ccloud: CCloudCliWrapper;
  
    async createApiKey(serviceAccountId: number, description: string): Promise<ApiKeySet> {
      let config = GetConfig();

      let cliOutput = await executeCli([
        "api-key",
        "create",
        "--resource", config.clusterId,
        "--environment", config.environmentId,
        "--service-account", serviceAccountId + "",
        "--description", description]
      );
  
      let cliObjects: any = parseSideColumns(cliOutput);
      let apiKeySet: ApiKeySet = { Key: cliObjects.APIKey, Secret: cliObjects.Secret }
  
      return apiKeySet;
    }
  
    async deleteApiKey(key: string): Promise<void> {
      await executeCli(["api-key", "delete", key]);
    }
    async getApiKeys(): Promise<ApiKey[]> {
      let cliOutput = await executeCli(["api-key", "list"]);
      let cliObjects = parse(cliOutput);
  
      let apiKeys = cliObjects.map(function (obj) {
        return { Key: obj.Key, Description: obj.Description, Owner: obj.Owner, Resource: obj.ResourceID } as ApiKey
      });
    
      return apiKeys;
    }
  
    constructor(ccloud: CCloudCliWrapper) {
      this.ccloud = ccloud;
    }
  }