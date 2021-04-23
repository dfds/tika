import { parse, parseSideColumns } from "./../parser";
import {executeCli } from "./executeCli";
import { GetConfig } from "../../config";

export class CcloudCluster {
    ccloud: CCloudCliWrapper;
  
    async list(): Promise<any[]> {
      let config = GetConfig();

      let result = await executeCli(["kafka", "cluster", "list", "--environment", config.environmentId]);
      parse(result);
      console.log("\n::SEP::\n");
      console.log(result);
  
      return result;
    }
  
    constructor(ccloud: CCloudCliWrapper) {
      this.ccloud = ccloud;
    }
  }