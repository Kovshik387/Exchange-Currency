import RecordItem from "@components/Volute/RecordItem";
import { Record } from "@models/Market";
import Routes from "@utils/routers";
import axios from "axios";
import { addDays, addMonths, addYears, format } from "date-fns";
import { useEffect, useState } from "react";
import { Button, Col, Container, Row, Spinner, Table } from "react-bootstrap";
import { useParams } from "react-router-dom";
import { ArrowDown, ArrowUp } from 'react-bootstrap-icons';
import {
    AreaChart, Area, CartesianGrid, XAxis, YAxis, Tooltip, ResponsiveContainer
} from 'recharts';

const CustomTooltip = ({ active, payload, label }: any) => {
    if (active && payload && payload.length) {
        return (
            <div className="custom-tooltip" style={{ backgroundColor: '#fff', padding: '10px', border: '1px solid #ccc', borderRadius: "10px" }}>
                <p>{label}</p>
                <p>{`Значение: ${payload[0].value}`}</p>
            </div>
        );
    }

    return null;
};

export default function VoluteDetailsPage() {
    const navigation = useParams();
    const dateFormatString = 'dd.MM.yyyy';
    const name = navigation["name"];

    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [dataRate, setDataRate] = useState<Record | null>();
    const [selectedPeriod, setSelectedPeriod] = useState<"5days" | "1month" | "1year">("1month");
    const [date1, setDate1] = useState(format(addMonths(new Date(), -1), dateFormatString));
    const [date2, setDate2] = useState(format(Date(), dateFormatString));
    const [sortedData, setSortedData] = useState<Record | null>();
    const [sortOrder, setSortOrder] = useState<"asc" | "desc">("desc");

    const fetchCurrencyData = async () => {
        setIsLoading(true);
        try {
            const response = await axios.get(Routes.ExchangeRate, {
                params: { date1: date1, date2: date2, name: name },
            });
            console.log(response);
            setDataRate(response.data);
            setSortedData(response.data,);
        } catch (error) {
            console.error('Error fetching currency data:', error);
        } finally {
            setIsLoading(false);
        }
    };

    const handlePeriodChange = (period: "5days" | "1month" | "1year") => {
        let newDate1 = "";
        const newDate2 = format(new Date(), 'dd.MM.yyyy');
        switch (period) {
            case "5days":
                newDate1 = format(addDays(new Date(), -5), dateFormatString);
                break;
            case "1month":
                newDate1 = format(addMonths(new Date(), -1), dateFormatString);
                break;
            case "1year":
                newDate1 = format(addYears(new Date(), -1), dateFormatString);
                break;
        }
        setDate1(newDate1);
        setDate2(newDate2);
        setSelectedPeriod(period);
    };

    const handleSort = () => {
        if (sortedData) {
            const order = sortOrder === "asc" ? "desc" : "asc";
            const sorted = [...sortedData.record].sort((a: any, b: any) => {
                const dateA = new Date(a.date.split('.').reverse().join('-'));
                const dateB = new Date(b.date.split('.').reverse().join('-'));
                return order === "asc" ? dateB.getTime() - dateA.getTime() : dateA.getTime() - dateB.getTime();
            });
            setSortedData({ ...sortedData, record: sorted });
            setSortOrder(order);
        }
    };

    useEffect(() => {
        fetchCurrencyData();
    }, [date1, date2]);

    return (
        <>
            <Container style={{ ...ContainerStyle, fontSize: "20px" }}>
                {isLoading ? (
                    <div style={{ textAlign: 'center' }}>
                        <Spinner animation="border" role="status">
                            <span className="sr-only"></span>
                        </Spinner>
                    </div>
                ) : (
                    <>
                        <h2>{dataRate?.record[0].name} | Номинал: {dataRate?.record[0].nominal}</h2>
                        <Row>
                            <Col xs={12} md={6}>
                                {dataRate && (
                                    <ResponsiveContainer height={400}>
                                        <AreaChart data={dataRate.record}>
                                            <defs>
                                                <linearGradient id="colorValue" x1="0" y1="0" x2="0" y2="1">
                                                    <stop offset="5%" stopColor="#8884d8" stopOpacity={0.8} />
                                                    <stop offset="95%" stopColor="#8884d8" stopOpacity={0} />
                                                </linearGradient>
                                            </defs>
                                            <CartesianGrid stroke="#ccc" />
                                            <XAxis dataKey="date" tick={{ fontSize: 12 }} interval="preserveStartEnd" />
                                            <YAxis domain={["auto"]} />
                                            <Tooltip content={<CustomTooltip />} />
                                            <Area type="monotone" dataKey="value" stroke="#8884d8" fillOpacity={1} fill="url(#colorValue)" />
                                        </AreaChart>
                                    </ResponsiveContainer>
                                )}
                                <Row>
                                    <Col xs={12} className="mb-3">
                                        <Button
                                            variant={selectedPeriod === "5days" ? "secondary" : "outline-secondary"}
                                            onClick={() => handlePeriodChange("5days")}
                                        >
                                            5 дней
                                        </Button>
                                        <Button
                                            variant={selectedPeriod === "1month" ? "secondary" : "outline-secondary"}
                                            onClick={() => handlePeriodChange("1month")}
                                            className="mx-2"
                                        >
                                            1 месяц
                                        </Button>
                                        <Button
                                            variant={selectedPeriod === "1year" ? "secondary" : "outline-secondary"}
                                            onClick={() => handlePeriodChange("1year")}
                                        >
                                            1 год
                                        </Button>
                                    </Col>
                                </Row>
                            </Col>


                            <Col xs={12} md={6}>
                                <div style={TableStyle}>
                                    <Table striped bordered hover >
                                        <thead>
                                            <tr>
                                                <th onClick={handleSort} style={{ cursor: 'pointer' }}>Дата {sortOrder === "desc" ? <ArrowDown width={15} /> : <ArrowUp width={15} />}</th>
                                                <th>Курс</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {sortedData?.record.map((currency, index) => (
                                                <RecordItem key={index} record={currency} />
                                            ))}
                                        </tbody>
                                    </Table>
                                </div>
                            </Col>
                        </Row>
                    </>
                )}
            </Container>
        </>
    )
}
const TableStyle: React.CSSProperties = {
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
    borderRadius: '8px',
    maxHeight: "400px",
    overflowY: "auto"
}

const ContainerStyle: React.CSSProperties = {
    padding: '20px',
    marginTop: "70px",
    marginBottom: "100px",
    overflowY: "auto",
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
    borderRadius: '8px',
    backgroundColor: '#fff',
    height: "85vh"
};