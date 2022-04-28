type Topic = {
    Name: string;
    PartitionCount: number;
    Configurations: {[key: string]: any}
}

type ServiceAccount = {
    Id: number;
    Name: string;
    Description: string;
}

type ApiKeySet = {
    Key: string;
    Secret: string;
}

type ApiKey = {
    Key: string;
    Description: string;
    Owner: string;
    Resource: string;
}

type AccessControlList = {
    UserId: string;
    ServiceAccountId: string;
    Permission: string;
    Operation: string;
    Resource: string;
    Name: string;
    Type: string;
}

interface ServiceAccounts {
    createServiceAccount(  
        name: string,
        description: string
    ): Promise<ServiceAccount>;

    deleteServiceAccount(
        id: number
    ): Promise<boolean>

    getServiceAccounts()
    : Promise<ServiceAccount[]>
}

interface ApiKeys {
    createApiKey(
        serviceAccountId: string,
        description: string
    ): Promise<ApiKeySet>;

    deleteApiKey(
        key: string
    ): Promise<void>;

    getApiKeys()
    : Promise<ApiKey[]>
}

interface AccessControlLists {
    createAccessControlList(
        serviceAccountId: number,
        allow: boolean,
        operation: string,
        topicPrefix: string,
        consumerGroupPrefix: string
    ): Promise<void>

    deleteAccessControlList(
        serviceAccountId: number,
        allow: boolean,
        operation: string,
        topicPrefix: string,
        consumerGroupPrefix: string
    ): Promise<void>

    getAccessControlLists()
    : Promise<AccessControlList[]>
}

interface Topics {
    getTopics(): Promise<string[]>
    describeTopic(name: string): Promise<Topic>
    createTopic(topic: Topic): Promise<void>
    deleteTopic(name: string): Promise<void>
    
}

interface Kafka{
    AccessControlLists : AccessControlLists;
    Topics: Topics;
}

interface CCloudCliWrapper{
    ServiceAccounts : ServiceAccounts;
    ApiKeys : ApiKeys;
    Kafka : Kafka;
}