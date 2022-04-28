export class CreateApiKey {
    key: string
    secret: string
}

export class ListApiKey{
    created: string
    description: string
    key: string
    owner_email: string
    owner_resource_id: string
    resource_id: string
    resource_type: string
}

export type ListApiKeys = Array<ListApiKey>