export default class ServiceAccount {
  id: number;
  name: string;
  description: string; // Currently isn't parsed properly (whitespace and space in general gets trimemd away)
}

export type ListServiceAccounts = Array<ServiceAccount>;