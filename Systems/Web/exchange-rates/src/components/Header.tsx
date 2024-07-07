import { CurrencyMarket } from '@models/Market';
import Routes from "@utils/routers"
import axios from 'axios';
import { addDays, format } from 'date-fns';
import { useEffect } from 'react';
import { Navbar, Nav, Container } from 'react-bootstrap';
import { AppDispatch, RootState } from "./../store/store";
import { useDispatch, useSelector } from "react-redux";
import { logout, setAuth, setLoggout } from './../store/account.Slice';
import { Link } from 'react-router-dom';

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
    const username = useSelector((state: RootState) => state.account.username);
    const isLoggedIn = useSelector((state: RootState) => state.account.isLoggedIn);
    const dispatch = useDispatch<AppDispatch>();

    const handleLogout = async () => {
        dispatch(logout());
        dispatch(setLoggout(false));
    };

    useEffect(() => {
        fetchCurrencyData();
        const storedUsername = localStorage.getItem('name');
        if (storedUsername) {
            dispatch(setAuth(storedUsername));
        }
    }, [dispatch]);

    return (
        <Navbar style={HeaderStyle} expand="lg" fixed="top">
            <Container>
                <Navbar.Brand as={Link} to="/">Мониторинг курсов валют</Navbar.Brand>
                <Navbar.Toggle aria-controls="basic-navbar-nav" />
                <Navbar.Collapse id="basic-navbar-nav">
                    <Nav className="me-auto">
                        <Nav.Link as={Link} to="/daily">
                            Курсы
                        </Nav.Link>
                        <Nav.Link as={Link} to="/currency">Динамика</Nav.Link>
                        <div className="d-flex align-items-center p-1">
                            <img src="/volutes/dollar.svg" height={25} alt="Dollar Icon" />
                            <h6 style={{ paddingTop: "6px" }}>&nbsp;{localStorage.getItem("dollar")}</h6>
                        </div>
                        <div className="d-flex align-items-center p-1">
                            <img src="/volutes/euro.svg" height={25} alt="Euro Icon" />
                            <h6 style={{ paddingTop: "6px" }}>&nbsp;{localStorage.getItem("euro")}</h6>
                        </div>
                    </Nav>
                    <Nav className="ms-auto" >
 
                    </Nav>
                    <Nav>
                        {
                            !isLoggedIn ?
                                <>
                                    <Navbar.Collapse className="me-auto">
                                        <Nav.Link as={Link} to="/signIn">Войти</Nav.Link>
                                        <Nav.Link as={Link} to="/signUp">Регистрация</Nav.Link>
                                    </Navbar.Collapse>
                                </>
                                :
                                <>
                                    <Navbar.Collapse className="me-auto">
                                        <Navbar.Text>
                                            Привет! {username} |
                                        </Navbar.Text>
                                        <Nav.Link as={Link} to={`/p/${localStorage.getItem("id")}`}>Профиль</Nav.Link>
                                        <Nav.Link onClick={handleLogout}>Выйти</Nav.Link>
                                    </Navbar.Collapse>
                                </>
                        }
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

