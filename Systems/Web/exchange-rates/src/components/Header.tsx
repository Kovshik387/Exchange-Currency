import { CurrencyMarket } from '@models/Market';
import Routes from "@utils/routers"
import axios from 'axios';
import { addDays, format } from 'date-fns';
import { useEffect } from 'react';
import { Navbar, Nav, Container } from 'react-bootstrap';

const fetchCurrencyData = async () => {
    if (localStorage.getItem("dollar") == null ||
        localStorage.getItem("euro") == null ||
        (localStorage.getItem("date") == null ||
            localStorage.getItem("date") != format(addDays(new Date(), 1), 'dd.MM.yyyy'))) {
        try {
            const response = await axios.get(Routes.ExchangeRates, {
                params: { date: format(addDays(new Date(), 1), 'dd.MM.yyyy') },
            });
            const data = response.data as CurrencyMarket;

            if (!data) return;

            localStorage.setItem("dollar", data.volute.find((x) => x.charCode == "USD")!.value.toFixed(2).toString())
            localStorage.setItem("euro", data.volute.find((x) => x.charCode == "EUR")!.value.toFixed(2).toString())
            localStorage.setItem("date", data.date);
        } catch (error) {
            console.error('Error fetching currency data:', error);
        }
    }
}

export default function Header() {
    useEffect(() => {
        fetchCurrencyData();
    })

    return (
        <Navbar style={HeaderStyle} expand="lg" fixed="top">
            <Container>
                <Navbar.Brand href="/">Мониторинг курсов валют</Navbar.Brand>
                <Navbar.Toggle aria-controls="basic-navbar-nav" />
                <Navbar.Collapse id="basic-navbar-nav">
                    <Nav className="me-auto">
                        <Nav.Link href="/daily">Курсы</Nav.Link>
                        <Nav.Link href="/currency">Динамика</Nav.Link>
                    </Nav>
                    <Nav className="ms-auto" >
                        <div className="d-flex align-items-center p-1">
                            <img src="/volutes/dollar.svg" height={25} alt="Dollar Icon" />
                            <h6 style={{ paddingTop: "6px" }}>&nbsp;{localStorage.getItem("dollar")}</h6>
                        </div>
                        <div className="d-flex align-items-center p-1">
                            <img src="/volutes/euro.svg" height={25} alt="Euro Icon" />
                            <h6 style={{ paddingTop: "6px" }}>&nbsp;{localStorage.getItem("euro")}</h6>
                        </div>
                    </Nav>
                </Navbar.Collapse>
            </Container>
        </Navbar>
    );
};

const HeaderStyle: React.CSSProperties = {
    backgroundColor: "#dadada",
    color: "black",
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.2)',
}