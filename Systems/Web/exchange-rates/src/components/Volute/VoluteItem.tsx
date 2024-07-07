import { Volute } from "@models/Market";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

interface CurrencyItemProps {
    currency: Volute;
}

export default function VoluteItem({ currency }: CurrencyItemProps) {
    const navigation = useNavigate();
    useEffect(() => {

    })

    return (
        <>
            <tr onClick={() => { navigation(`/details/${currency.id}`) }}>
                <td>{currency.numCode}</td>
                <td>{currency.charCode}</td>
                <td>{currency.nominal}</td>
                <td>{currency.name}</td>
                <td>{currency.value}</td>
            </tr>
        </>
    );
};
