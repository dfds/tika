export class NotConnectedApiKeys implements ApiKeys {
    private static instance: NotConnectedApiKeys;


    public static getInstance(): ApiKeys {
        if (!NotConnectedApiKeys.instance) {
            NotConnectedApiKeys.instance = new NotConnectedApiKeys();
        }

        return NotConnectedApiKeys.instance;
    }

    apiKeys: ApiKey[];

    constructor() {
        this.apiKeys = [];
    }
    async createApiKey(serviceAccountId: number, description: string): Promise<ApiKeySet> {
        let key = NotConnectedApiKeys.createRandomString(16, true);

        let apiKey: ApiKey = {
            Key: key,
            Description: description,
            Owner: serviceAccountId.toString(),
            Resource: ""
        };
        this.apiKeys.push(apiKey);


        let apiKeySet: ApiKeySet =
        {
            Key: key,
            Secret: NotConnectedApiKeys.createRandomString(64, false)
        };

        return apiKeySet;
    }

    async deleteApiKey(key: string): Promise<void> {
        this.apiKeys = this.apiKeys.filter(a => a.Key !== key);
    }
    
    async getApiKeys(): Promise<ApiKey[]> {
        return this.apiKeys;
    }

    private static createRandomString(length: number, onlyUppercaseAndNumb: boolean): string {
        let result: string = '';
        let characters = onlyUppercaseAndNumb ?
            'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789' :
            'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789/';

        let charactersLength = characters.length;
        for (var i = 0; i < length; i++) {
            result += characters.charAt(Math.floor(Math.random() * charactersLength));
        }
        return result;
    }
}