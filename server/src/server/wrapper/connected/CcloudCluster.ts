import { parse, parseSideColumns } from "./../parser";
import {executeCli } from "./executeCli";
import { GetConfig } from "../../config";
import { Deserializer, ConcatOutput } from "../utils";
import Clusters from "../model/clusters";

export class CcloudCluster {
    ccloud: CCloudCliWrapper;
  
    async list(): Promise<any[]> {
      let config = GetConfig();

      let result = await executeCli(["kafka", "cluster", "list", "--environment", config.environmentId, "--output", "json"]);
      
      let combinedResult = ConcatOutput(result);
      let deserializedResult : Clusters;
      try {
        deserializedResult = Deserializer<Clusters>(combinedResult);
      } catch (error) {
        return error;
      }
  
      return deserializedResult;
    }
  
    constructor(ccloud: CCloudCliWrapper) {
      this.ccloud = ccloud;
    }
  }