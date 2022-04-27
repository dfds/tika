export class Cluster {
  availability: string
  id: string
  name: string
  provider: string
  region: string
  status: string
  type: string
}

type Clusters = Array<Cluster>;

export default Clusters;