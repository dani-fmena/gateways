
export interface Gateway {
  id: number;
  name: string;
  serialNumber: string;
  ipAddress: string;
  peripheralsAssociated: number;
}


export function generateFakeData(): Gateway {
  return {
    id: Math.random(),
    name: 'N' + Math.random(),
    serialNumber: 'SN' + Math.random(),
    ipAddress: '127.0.0.1',
    peripheralsAssociated: Math.random(),
  };
}
