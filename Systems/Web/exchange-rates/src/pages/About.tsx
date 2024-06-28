import { Col, Container, Row } from "react-bootstrap";

export default function AboutPage() {
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
                        <div className="row">
                            <div className="col-md-6">Австралийский доллар</div>
                            <div className="col-md-6">Азербайджанский манат</div>
                            <div className="col-md-6">Армянский драм</div>
                            <div className="col-md-6">Белорусский рубль</div>
                            <div className="col-md-6">Болгарский лев</div>
                            <div className="col-md-6">Бразильский реал</div>
                            <div className="col-md-6">Венгерский форинт</div>
                            <div className="col-md-6">Вон Республики Корея</div>
                            <div className="col-md-6">Вьетнамский донг</div>
                            <div className="col-md-6">Гонконгский доллар</div>
                            <div className="col-md-6">Грузинский лари</div>
                            <div className="col-md-6">Датская крона</div>
                            <div className="col-md-6">Дирхам ОАЭ</div>
                            <div className="col-md-6">Доллар США</div>
                            <div className="col-md-6">Евро</div>
                            <div className="col-md-6">Египетский фунт</div>
                            <div className="col-md-6">Индийская рупия</div>
                            <div className="col-md-6">Индонезийская рупия</div>
                            <div className="col-md-6">Казахстанский тенге</div>
                            <div className="col-md-6">Канадский доллар</div>
                            <div className="col-md-6">Катарский риал</div>
                            <div className="col-md-6">Киргизский сом</div>
                            <div className="col-md-6">Китайский юань</div>
                            <div className="col-md-6">Молдавский лей</div>
                            <div className="col-md-6">Новозеландский доллар</div>
                            <div className="col-md-6">Новый туркменский манат</div>
                            <div className="col-md-6">Норвежская крона</div>
                            <div className="col-md-6">Польский злотый</div>
                            <div className="col-md-6">Румынский лей</div>
                            <div className="col-md-6">СДР</div>
                            <div className="col-md-6">Сербский динар</div>
                            <div className="col-md-6">Сингапурский доллар</div>
                            <div className="col-md-6">Таджикский сомони</div>
                            <div className="col-md-6">Таиландский бат</div>
                            <div className="col-md-6">Турецкая лира</div>
                            <div className="col-md-6">Узбекский сум</div>
                            <div className="col-md-6">Украинская гривна</div>
                            <div className="col-md-6">Чешская крона</div>
                            <div className="col-md-6">Шведская крона</div>
                            <div className="col-md-6">Швейцарский франк</div>
                            <div className="col-md-6">Южноафриканский рэнд</div>
                            <div className="col-md-6">Японская иена</div>
                            <div className="col-md-6">Фунт стерлингов Соединенного королевства</div>
                        </div>
                    </Col>
                </Row>



            </Container>
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

