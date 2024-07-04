import { CurrencyMarket } from '@models/Market';
import { Table } from 'react-bootstrap';
import VoluteItem from "@components/Volute/VoluteItem"

interface CurrencyTableProps {
    data: CurrencyMarket;
}

export default function VoluteTable({ data }: CurrencyTableProps) {
    return (
        <Table striped bordered hover>
            <thead>
                <tr>
                    <th>Цифр. код</th>
                    <th>Букв. код</th>
                    <th>Единиц</th>
                    <th>Валюта</th>
                    <th>Курс</th>
                </tr>
            </thead>
            <tbody>
                {
                    data.volute.map((currency) => (
                        <VoluteItem
                            key={currency.id}
                            currency={currency}
                        />
                    ))
                }
            </tbody>
        </Table>
    );
};


