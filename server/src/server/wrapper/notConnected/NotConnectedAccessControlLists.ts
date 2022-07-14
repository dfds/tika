export class NotConnectedAccessControlLists implements AccessControlLists {

  private static instance: NotConnectedAccessControlLists;


  public static getInstance(): AccessControlLists {
    if (!NotConnectedAccessControlLists.instance) {
      NotConnectedAccessControlLists.instance = new NotConnectedAccessControlLists();
    }

    return NotConnectedAccessControlLists.instance;
  }

  accessControlLists: AccessControlList[];

  constructor() {
    this.accessControlLists = [];
  }

  async createAccessControlList(
    serviceAccountId: number,
    allow: boolean,
    operation: string,
    topicPrefix: string,
    consumerGroupPrefix: string
  ): Promise<void> {

    var accessControlList = this.createAccessControlListObject(
      serviceAccountId,
      allow,
      operation,
      topicPrefix,
      consumerGroupPrefix
    );


    if (this.accessControlLists
      .some((a) => this.areEqual(a, accessControlList))) { return; }
    
      this.accessControlLists.push(accessControlList);
  }

  async deleteAccessControlList(
    serviceAccountId: number,
    allow: boolean,
    operation: string,
    topicPrefix: string,
    consumerGroupPrefix: string
  ): Promise<void> {

    let accessControlList = this.createAccessControlListObject(
      serviceAccountId,
      allow,
      operation,
      topicPrefix,
      consumerGroupPrefix
    );

    this.accessControlLists = this.accessControlLists
      .filter(a => this.areEqual(a, accessControlList) == false);
  }

  async getAccessControlLists(): Promise<AccessControlList[]> {
    return this.accessControlLists;
  }

  private areEqual(a: AccessControlList, b: AccessControlList): boolean {
    return  a.ServiceAccountId === b.ServiceAccountId &&
      a.Permission === b.Permission &&
      a.Operation === b.Operation &&
      a.Resource === b.Resource &&
      a.Name === b.Name &&
      a.Type === b.Type
  }

  private isCluster(consumerGroupPrefix: string): boolean {
    return consumerGroupPrefix === "CLUSTER"
  }

  private createAccessControlListObject(serviceAccountId: number,
    allow: boolean,
    operation: string,
    topicPrefix: string,
    consumerGroupPrefix: string): AccessControlList {
    let name = this.isCluster(consumerGroupPrefix) ? "kafka-cluster" : "";
    if (topicPrefix !== undefined) { name = topicPrefix; }
    else if (consumerGroupPrefix !== undefined) { name = consumerGroupPrefix }
    let accessControlList: AccessControlList = {
      UserId: serviceAccountId + "",
      ServiceAccountId: serviceAccountId + "",
      Permission: allow ? "ALLOW" : "DENY",
      Operation: operation,
      Resource: topicPrefix !== undefined ? "TOPIC" : consumerGroupPrefix !== undefined ? "GROUP" : "CLUSTER",
      Name: name,
      Type: topicPrefix !== undefined || consumerGroupPrefix !== undefined ? "PREFIXED" : "LITERAL"
    }

    return accessControlList;
  }

}