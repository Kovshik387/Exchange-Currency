export interface Volute {
  id: string;
  numCode: number;
  charCode: string;
  nominal: number;
  name: string;
  value: number;
  vunitRate: number;
  date: string
}
export interface Record {
  date: any;
  value: any;
  record: Volute[]
}

export interface CurrencyMarket {
    date: string;
    name: string;
    volute: Volute[];
}
