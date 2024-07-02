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

export interface CurrencyMarket {
    date: string;
    name: string;
    volute: Volute[];
}

export interface Record {
  record: Volute[]
}