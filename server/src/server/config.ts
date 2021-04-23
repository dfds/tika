export class Config {
  clusterId: string;
  environmentId: string;
}

export function GetConfig() : Config {
  let payload = new Config();
  payload.clusterId = process.env.TIKA_CCLOUD_CLUSTER_ID;
  payload.environmentId = process.env.TIKA_CCLOUD_ENVIRONMENT_ID

  return payload;
}