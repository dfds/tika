import { parse } from "./../parser";
import { executeCli } from "./executeCli";
import { GetConfig } from "../../config";

export class CcloudAccessControlLists implements AccessControlLists {
    async getAccessControlLists(): Promise<AccessControlList[]> {
        let config = GetConfig();
        let result = await executeCli(["kafka", "acl", "list", "--cluster", config.clusterId, "--environment", config.environmentId]);
        let resultObjects = parse(result) as AccessControlList[];

        resultObjects.forEach(elem => {
            elem.ServiceAccountId = elem.ServiceAccountId.split(':')[1];
        });

        return resultObjects;
    }

    async createAccessControlList(
        serviceAccountId: number,
        allow: boolean,
        operation: string,
        topicPrefix: string,
        consumerGroupPrefix: string
    ): Promise<void> {
        
        let command = this.createCommand(
            "create",
            serviceAccountId,
            allow,
            operation,
            topicPrefix,
            consumerGroupPrefix
        );

        await executeCli(command);
    }

    async deleteAccessControlList(
        serviceAccountId: number,
        allow: boolean,
        operation: string,
        topicPrefix: string,
        consumerGroupPrefix: string
    ): Promise<void> {

        let command = this.createCommand(
            "delete",
            serviceAccountId,
            allow,
            operation,
            topicPrefix,
            consumerGroupPrefix
        );

        await executeCli(command);
    }

    private createCommand(
        createOrDelete: string,
        serviceAccountId: number,
        allow: boolean,
        operation: string,
        topicPrefix: string,
        consumerGroupPrefix: string
    ): string[] {
        let config = GetConfig();
        let command = [
            "kafka", "acl", createOrDelete,
            "--cluster", config.clusterId,
            "--environment", config.environmentId,
            "--service-account", serviceAccountId + "",
            "--operation", operation
        ];

        command.push(allow ? "--allow" : "--deny");

        if (consumerGroupPrefix !== undefined && 0 < consumerGroupPrefix.length) {
            command.push("--consumer-group");
            command.push(consumerGroupPrefix);
            command.push("--prefix");
        }
        else if (topicPrefix !== undefined && 0 < topicPrefix.length) {
            command.push("--topic");
            command.push(topicPrefix);
            command.push("--prefix");
        } else {
            command.push("--cluster-scope");
        }

        return command;
    }
}