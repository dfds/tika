import { parse, parseSideColumns } from "./../parser";
import { executeCli } from "./executeCli";
import { CliException, ServiceAccountAlreadyExistsException } from "./../model/error";
import { Deserializer, ConcatOutput } from "../utils";
import { ListServiceAccounts } from "../model/service-account";

export class CcloudServiceAccount implements ServiceAccounts {
  ccloud: CCloudCliWrapper;

  async getServiceAccounts(): Promise<ServiceAccount[]> {
    let result = await executeCli(["iam", "service-account", "list", "--output", "json"]);
    let combinedResult = ConcatOutput(result);
    let deserializedResult : ListServiceAccounts;
    try {
        deserializedResult = Deserializer<ListServiceAccounts>(combinedResult);
    } catch (error) {
        return error;
    }

    return deserializedResult.map(t => {
      let obj = {
        Name: t.name,
        Id: t.id,
        Description: t.description
      };
      return obj;
    })
  }

  async createServiceAccount(accountName: string, description: string = ""): Promise<ServiceAccount> {
    let cliResult;
    try {
      cliResult = await executeCli(["service-account", "create", accountName, "--description", description]);
    }
    catch (error) {
      if (error.name.valueOf() !== "CliException") {
        throw (error);
      }

      if (error.consoleLines.some((l: string): boolean => l.includes("Service name is already in use"))) {
        let existingServicesAccounts = await this.getServiceAccounts();

        let existingServicesAccount = existingServicesAccounts.find(s => s.Name === accountName);

        let isTheSame = existingServicesAccount.Description === description;

        if (isTheSame) {
          return existingServicesAccount;
        }

        throw new ServiceAccountAlreadyExistsException();
      }

      throw (error);
    }



    let result = parseSideColumns(cliResult);

    return (result as any) as ServiceAccount;
  }

  async deleteServiceAccount(accountId: number): Promise<boolean> {
    await executeCli(["service-account", "delete", accountId.toString()]);
    return true;
  }

  async update(description: string): Promise<boolean> {
    throw new Error("Not implemented");
  }

  constructor(ccloud: CCloudCliWrapper) {
    this.ccloud = ccloud;
  }
}