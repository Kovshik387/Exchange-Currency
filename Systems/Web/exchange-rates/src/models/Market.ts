export interface Volute {
  id: string;
  numCode: number;
  charCode: string;
  nominal: number;
  name: string;
  value: number;
  vunitRate: number;
}

export interface CurrencyMarket {
    date: string;
    name: string;
    volute: Volute[];
  }