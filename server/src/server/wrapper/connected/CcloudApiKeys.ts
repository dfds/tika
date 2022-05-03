import { parse, parseSideColumns } from "./../parser";
import {executeCli } from "./executeCli";
import { GetConfig } from "../../config";
import { Deserializer, ConcatOutput } from "../utils";
import { CreateApiKey, ListApiKeys } from "../model/api-keys";

export class CcloudApiKeys implements ApiKeys {
    ccloud: CCloudCliWrapper;
  
    async createApiKey(serviceAccountId: string, description: string): Promise<ApiKeySet> {
      let config = GetConfig();

      let cliOutput = await executeCli([
        "api-key",
        "create",
        "--resource", config.clusterId,
        "--environment", config.environmentId,
        "--service-account", serviceAccountId,
        "--description", description, "--output", "json"]
      );
  
      let cliObjects: any = parseSideColumns(cliOutput);
      let apiKeySet: ApiKeySet = { Key: cliObjects.APIKey, Secret: cliObjects.Secret }

      let combinedResult = ConcatOutput(cliOutput);
      let deserializedResult : CreateApiKey;
      try {
          deserializedResult = Deserializer<CreateApiKey>(combinedResult);
      } catch (error) {
          return error;
      }
  
      return {
        Key: deserializedResult.key,
        Secret: deserializedResult.secret
      };
    }
  
    async deleteApiKey(key: string): Promise<void> {
      await executeCli(["api-key", "delete", key]);
    }

    async getApiKeys(): Promise<ApiKey[]> {
      let cliOutput = await executeCli(["api-key", "list", "--output", "json"]);
  
      let combinedResult = ConcatOutput(cliOutput);
      let deserializedResult : ListApiKeys;
      try {
          deserializedResult = Deserializer<ListApiKeys>(combinedResult);
      } catch (error) {
          return error;
      }

      return deserializedResult.map(t => {
        let obj = {
          Key: t.key,
          Description: t.description,
          Owner: t.owner_resource_id,
          Resource: t.resource_id
        };
        return obj;
      })      
    }
  
    constructor(ccloud: CCloudCliWrapper) {
      this.ccloud = ccloud;
    }
  }