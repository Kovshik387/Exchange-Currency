import React, { useEffect, useState } from 'react';
import { useSearchParams } from 'react-router-dom';
import { Col, Container, Row, Spinner, Button, InputGroup, Badge, CloseButton, Stack, Toast } from 'react-bootstrap';
import { Typeahead } from 'react-bootstrap-typeahead';
import axios from 'axios';
import { format, addMonths, addDays, addYears } from 'date-fns';
import {
    AreaChart, Area, CartesianGrid, XAxis, YAxis, Tooltip, ResponsiveContainer, Legend
} from 'recharts';
import Routes from '@utils/routers';
import { CurrencyMarket, Record, Volute } from '@models/Market';
import { Eye, EyeSlash } from 'react-bootstrap-icons';

const COLORS = ['#8884d8', '#82ca9d', '#ffc658', '#ff7300', '#ff0000'];

interface CurrencyData {
    date: string;
    [key: string]: number | string;
}

interface ApiResponse {
    record: Record[];
}

export default function ComparisonPage() {
    const [currencyData, setCurrencyData] = useState<CurrencyMarket | null>(null);
    const [dataRates, setDataRates] = useState<CurrencyData[]>([]);
    const [date1, setDate1] = useState<string>(format(addMonths(new Date(), -1), 'dd.MM.yyyy'));
    const [date2, setDate2] = useState<string>(format(new Date(), 'dd.MM.yyyy'));
    const [selectedPeriod, setSelectedPeriod] = useState<"5days" | "1month" | "1year">("1month");
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const [currencies, setCurrencies] = useState<string[]>([]);
    const [hiddenCurrencies, setHiddenCurrencies] = useState<Set<string>>(new Set());
    const [newCurrency, setNewCurrency] = useState<Volute | null>(null);
    const [showCopyNotification, setShowCopyNotification] = useState(false);
    const dateFormatString = 'dd.MM.yyyy';

    const [searchParams] = useSearchParams();

    useEffect(() => {
        const date1Param = searchParams.get('date1');
        const date2Param = searchParams.get('date2');
        const currenciesParam = searchParams.get('currencies');

        if (date1Param) setDate1(date1Param);
        if (date2Param) setDate2(date2Param);
        if (currenciesParam) setCurrencies(currenciesParam.split(','));
    }, [searchParams]);

    const fetchVoluteData = async () => {
        const response = await axios.get(Routes.ExchangeRates, {
            params: { date: format(new Date(), 'dd.MM.yyyy') },
        });
        setCurrencyData(response.data);
    };

    const fetchCurrencyData = async (currencyName: string): Promise<Record[]> => {
        try {
            const response = await axios.get<ApiResponse>(Routes.ExchangeRate, {
                params: { date1: date1, date2: date2, name: currencyName },
            });
            return response.data.record;
        } catch (error) {
            console.error('Error fetching currency data:', error);
            return [];
        }
    };

    const fetchData = async () => {
        setIsLoading(true);
        console.log('Fetching data for date range:', date1, date2);

        try {
            const allData = await Promise.all(currencies.map(name => fetchCurrencyData(name)));
            const transformedData: CurrencyData[] = [];

            allData.forEach((currencyData, index) => {
                currencyData.forEach((dataPoint) => {
                    const existingDataPoint = transformedData.find(item => item.date === dataPoint.date);
                    if (existingDataPoint) {
                        existingDataPoint[currencies[index]] = dataPoint.value;
                    } else {
                        transformedData.push({
                            date: dataPoint.date,
                            [currencies[index]]: dataPoint.value
                        });
                    }
                });
            });

            setDataRates(transformedData);
            console.log('Data fetched and transformed:', transformedData);
        } catch (error) {
            console.error('Error in fetchData:', error);
        }

        setIsLoading(false);
    };

    useEffect(() => {
        fetchVoluteData();
        fetchData();
    }, [date1, date2, currencies]);

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

    const handleAddCurrency = () => {
        if (newCurrency && !currencies.includes(newCurrency.id)) {
            setCurrencies([...currencies, newCurrency.id]);
            setNewCurrency(null);
        }
    };

    const handleRemoveCurrency = (currency: string) => {
        setCurrencies(currencies.filter(c => c !== currency));
        setHiddenCurrencies(hiddenCurrencies => {
            const newHiddenCurrencies = new Set(hiddenCurrencies);
            newHiddenCurrencies.delete(currency);
            return newHiddenCurrencies;
        });
    };

    const handleToggleVisibility = (currency: string) => {
        setHiddenCurrencies(hiddenCurrencies => {
            const newHiddenCurrencies = new Set(hiddenCurrencies);
            if (newHiddenCurrencies.has(currency)) {
                newHiddenCurrencies.delete(currency);
            } else {
                newHiddenCurrencies.add(currency);
            }
            return newHiddenCurrencies;
        });
    };

    const currencyIdToName = new Map<string, string>();
    currencyData?.volute.forEach(volute => {
        currencyIdToName.set(volute.id, volute.name);
    });

    const generateShareableLink = () => {
        const baseUrl = window.location.href.split('?')[0];
        const params = new URLSearchParams({
            date1: date1,
            date2: date2,
            currencies: currencies.join(',')
        }).toString();
        return `${baseUrl}?${params}`;
    };

    const handleCopyLink = () => {
        const link = generateShareableLink();
        console.log('Generated link:', link); // Debug statement
        navigator.clipboard.writeText(link).then(() => {
            console.log('Link copied to clipboard'); // Debug statement
            setShowCopyNotification(true);
            setTimeout(() => setShowCopyNotification(false), 2000);
        }).catch(err => {
            console.error('Error copying link:', err);
        });
    };

    return (
        <Container style={{ ...ContainerStyle, fontSize: "20px" }}>
            {isLoading ? (
                <div style={{ textAlign: 'center' }}>
                    <Spinner animation="border" role="status">
                        <span className="sr-only"></span>
                    </Spinner>
                </div>
            ) : (
                <>
                    <Row className="mb-4">
                        <Col xs={12}>
                            {currencies.map((currency, index) => (
                                <Badge key={index} bg="secondary" className="mb-2 me-2">
                                    <Stack direction="horizontal" gap={2}>
                                        {currencyIdToName.get(currency) || currency}
                                        <Stack direction="horizontal" gap={2}>
                                            <Button variant="secondary" onClick={() => handleToggleVisibility(currency)}>
                                                {hiddenCurrencies.has(currency) ? <EyeSlash style={{ paddingBottom: "2px" }} /> : <Eye style={{ paddingBottom: "2px" }} />}
                                            </Button>
                                            <CloseButton className='py-2' onClick={() => handleRemoveCurrency(currency)} />
                                        </Stack>
                                    </Stack>
                                </Badge>
                            ))}
                        </Col>
                    </Row>
                    <Row className="mb-3">
                        <Col xs={12}>
                            <InputGroup>
                                <Typeahead
                                    id="currency-selector"
                                    labelKey="name"
                                    options={currencyData ? currencyData.volute : []}
                                    placeholder="Добавить валюту"
                                    selected={newCurrency ? [newCurrency] : []}
                                    onChange={(selected: any) => setNewCurrency(selected[0] || null)}
                                />
                                <Button variant="outline-secondary" hidden={currencies.length == 5} onClick={handleAddCurrency}>Добавить</Button>
                            </InputGroup>
                        </Col>
                    </Row>
                    <ResponsiveContainer width="100%" height={500}>
                        <AreaChart data={dataRates}>
                            <CartesianGrid strokeDasharray="3 3" />
                            <XAxis dataKey="date" />
                            <YAxis domain={["auto"]} />
                            <Tooltip />
                            <Legend />
                            {currencies.map((currency, index) => (
                                !hiddenCurrencies.has(currency) && (
                                    <Area
                                        key={currency}
                                        type="monotone"
                                        dataKey={currency}
                                        name={currencyIdToName.get(currency) || currency}
                                        stroke={COLORS[index % COLORS.length]}
                                        fillOpacity={1}
                                        fill={`url(#colorValue${index})`}
                                    />
                                )
                            ))}
                        </AreaChart>
                    </ResponsiveContainer>
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
                        <Col>
                            <Button
                                variant="outline-secondary" onClick={handleCopyLink}
                            >
                                Поделиться ссылкой
                            </Button>
                        </Col>
                    </Row>
                    <Toast
                        onClose={() => setShowCopyNotification(false)}
                        show={showCopyNotification}
                        delay={2000}
                        autohide
                        style={{
                            position: 'fixed',
                            bottom: 80,
                            right: 20,
                            minWidth: '200px',
                            zIndex: 1050
                        }}
                    >
                        <Toast.Body>Ссылка скопирована!</Toast.Body>
                    </Toast>
                </>
            )}
        </Container>
    );
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
