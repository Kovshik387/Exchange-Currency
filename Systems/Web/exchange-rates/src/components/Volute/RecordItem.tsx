import { Volute } from "@models/Market";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

interface CurrencyItemProps {
    record: Volute;
}

export default function RecordItem({ record }: CurrencyItemProps) {
    const navigation = useNavigate();
    useEffect(() => {

    })
    
    return (
        <>
            <tr onClick={() => {navigation(`/details/${record.id}`)}}>
                <td>{record.date}</td>
                <td>{record.value}</td>
            </tr>
        </>
    );
};
