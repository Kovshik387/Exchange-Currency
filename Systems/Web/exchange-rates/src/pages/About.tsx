import { CurrencyMarket } from "@models/Market";
import axios from "axios";
import { format } from "date-fns";
import { useEffect, useState } from "react";
import { Col, Container, Row, Spinner } from "react-bootstrap";
import Routes from "@utils/routers";

export default function AboutPage() {
    const [currencyData, setCurrencyData] = useState<CurrencyMarket | null>();
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const selectedDate = new Date();
    const fetchCurrencyData = async (date: Date) => {
        setIsLoading(true);
        try {
            const response = await axios.get(Routes.ExchangeRates, {
                params: { date: format(date, 'dd.MM.yyyy') },
            });
            setCurrencyData(response.data);
        } catch (error) {
            console.error('Error fetching currency data:', error);
        } finally {
            setIsLoading(false);
        }
    };

    useEffect(() => {
        fetchCurrencyData(selectedDate);
    }, []);

    return (
        <>
            <Container style={{ ...ContainerStyle, fontSize: "20px" }}>
                <h1>Добро пожаловать на сайт обмена валют</h1>

                <Row>
                    <Col>
                        <h2>Особенности сервиса</h2>
                        <ul>
                            <li>
                                <strong>
                                    Актуальные данные:
                                </strong> Мы получаем данные напрямую от проверенных источников, чтобы обеспечить точность и надежность информации.
                            </li>
                            <li>
                                <strong>
                                    Исторические данные:
                                </strong> Возможность просматривать курсы валют за различные даты.
                            </li>
                            <li>
                                <strong>
                                    Простой интерфейс:
                                </strong> Интуитивно понятный интерфейс, позволяющий легко находить нужную информацию.
                            </li>
                            <li>
                                <strong>
                                    Высокая скорость работы:
                                </strong> Быстрый доступ к данным благодаря современным технологиям обработки и кэширования информации
                            </li>
                            <li>
                                <strong>
                                    Git:
                                </strong>
                                <a>
                                    &nbsp; https://github.com/Kovshik387/Exchange-Currency-DSR
                                </a>
                            </li>
                        </ul>
                    </Col>
                    <Col>
                        <h2>Список валют</h2>
                        {isLoading ? (
                            <div style={{ textAlign: 'center' }}>
                                <Spinner animation="border" role="status">
                                    <span className="sr-only"></span>
                                </Spinner>
                            </div>
                        ) : (
                            <div className="row">
                                {currencyData?.volute.map((item) => (
                                    <div className="col-md-6">
                                        <a href={`/details/${item.id}`}>
                                            {item.name}
                                        </a>
                                    </div>
                                ))}
                            </div>
                        )}
                    </Col>
                </Row>



            </Container >
        </>
    )
}

const ContainerStyle: React.CSSProperties = {
    padding: '20px',
    marginTop: "60px",
    marginBottom: "100px",
    overflowY: "auto",
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
    borderRadius: '8px',
    backgroundColor: '#fff',
    height: "85vh"
};

