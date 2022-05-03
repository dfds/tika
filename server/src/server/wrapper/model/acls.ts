export class ListAcl {
    operation: string
    pattern_type: string
    permission: string
    principal: string
    resource_name: string
    resource_type: string
}

export type ListAcls = Array<ListAcl>
