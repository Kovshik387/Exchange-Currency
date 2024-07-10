import { CurrencyMarket } from "@models/Market";
import axios from "axios";
import { addDays, format } from "date-fns";
import { useEffect, useState } from "react";
import { Card, Col, Container, Row, Spinner } from "react-bootstrap";
import Routes from "@utils/routers";
import { Link } from "react-router-dom";

export default function AboutPage() {
    const [currencyData, setCurrencyData] = useState<CurrencyMarket | null>();
    const [isLoading, setIsLoading] = useState<boolean>(false);
    const selectedDate = addDays(new Date(), 1);
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

    const cardStyle: React.CSSProperties = {
        backgroundColor: '#f8f9fa',
        color: '#000',
        transform: 'translateY(5px)',
        height: "100px",
        transition: 'transform 0.3s ease, box-shadow 0.3s ease'
    };

    const cardHoverStyle: React.CSSProperties = {
        transform: 'translateY(-5px)',
        color: "#8884d8",
        boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)'
    };

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
                                    Исходные файлы:
                                </strong>
                                <a href="https://github.com/Kovshik387/Exchange-Currency-DSR" style={{ textDecoration: "none", color: "#8884d8" }}>
                                    &nbsp;Git
                                </a>
                            </li>
                        </ul>
                    </Col>
                    <Col >
                        <h2>Список валют</h2>
                        {isLoading ? (
                            <div style={{ textAlign: 'center' }}>
                                <Spinner animation="border" role="status">
                                    <span className="sr-only"></span>
                                </Spinner>
                            </div>
                        ) : (
                            <Row style={{ height: "550px", overflowY: "auto", boxShadow: '1px 2px 2px rgba(0, 0, 0, 0.1)', borderRadius: "5px" }}>
                                {currencyData?.volute.map((item) => (
                                    <Col md={4} key={item.id} style={{ padding: "10px" }}>
                                        <Link
                                            to={`/details/${item.id}`}
                                            style={{
                                                textAlign: 'center',
                                                textDecoration: 'none',
                                            }}
                                        >
                                            <Card
                                                onMouseEnter={(e) => Object.assign(e.currentTarget.style, cardHoverStyle)}
                                                onMouseLeave={(e) => Object.assign(e.currentTarget.style, cardStyle)}
                                                style={cardStyle}
                                            >
                                                <Card.Body
                                                >
                                                    <Card.Title>{item.name}</Card.Title>
                                                </Card.Body>
                                            </Card>
                                        </Link>
                                    </Col>
                                ))}
                            </Row>
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

