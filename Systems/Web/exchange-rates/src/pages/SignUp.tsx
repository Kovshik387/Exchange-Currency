import SignUpModel from "@models/AuthSignUp";
import { registration, setAuth } from "./../store/account.Slice";
import { AppDispatch } from "@stores/store";
import React from "react";
import { Button, Col, Container, Form, InputGroup, Row, Toast } from "react-bootstrap";
import { EyeSlashFill, Eye } from "react-bootstrap-icons";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";

export default function SignUpPage() {
    const dispatch = useDispatch<AppDispatch>();
    const navigate = useNavigate();

    const [showPassword, setShowPassword] = React.useState(false);
    const [password, setPassword] = React.useState("");
    const [showErrorNotification, setShowErrorNotification] = React.useState(false);
    const [errorMessage, setErrorMessage] = React.useState("");

    if (localStorage.getItem('name') != null) navigate('/');

    const handleError = (error: string) => {
        navigator.clipboard.writeText(error).then(() => {
            setErrorMessage(error);
            setShowErrorNotification(true);
            setTimeout(() => setShowErrorNotification(false), 2000);
        }).catch(err => {
            console.error('Error showing:', err);
        });
    };

    const handleSubmit = async (event: React.SyntheticEvent<HTMLFormElement>) => {
        event.preventDefault();
        event.stopPropagation();

        const form = event.currentTarget;
        const formElements = form.elements as typeof form.elements & {
            email: { value: String },
            password: { value: String },
            username: { value: String },
            name: { value: String },
            patronymic: { value: String | null },
            surname: { value: String }
        }

        const account: SignUpModel = {
            email: formElements.email.value,
            password: formElements.password.value,
            name: formElements.name.value,
            patronymic: formElements.patronymic.value,
            surname: formElements.surname.value
        }

        const requestAccount = { model: account }
        const resultAction = await dispatch(registration(requestAccount));
        console.log(resultAction)
        if (registration.rejected.match(resultAction)) {
            const errorMessage = resultAction.payload;
            console.log(errorMessage);
            window.alert(errorMessage![0]);
        } else if (registration.fulfilled.match(resultAction)) {
            const data = resultAction.payload;

            if (data.data !== null) {
                localStorage.setItem("id", data.data.id!.toString());
                localStorage.setItem("accessToken", data.data.accessToken!);
                localStorage.setItem("refreshToken", data.data.refreshToken!);
                localStorage.setItem("name", data.data.name);
                dispatch(setAuth(data.data.name));
                navigate('/');
            }
            handleError(data.errorMessage);
        }
    }

    return (<>
        <Container style={{ ...ContainerStyle, fontSize: "20px", marginTop: "220px", maxWidth: "500px", height: "100%" }}>
            <h3>Регистрация</h3>
            <Form onSubmit={handleSubmit} >
                <Row>
                    <Col style={{ padding: "10px" }}>
                        <Form.Group className="mb-3" >
                            <Form.Label>Почта</Form.Label>
                            <InputGroup hasValidation >
                                <Form.Control type="email" name="email" id="email" placeholder="name@example.com" required />
                            </InputGroup>

                            <Form.Label>Пароль</Form.Label>
                            <InputGroup hasValidation >
                                <Form.Control type={showPassword ? "text" : "password"} value={password} onChange={(e => setPassword(e.target.value))} name="password" id="passwordVisible" placeholder="Password" />
                                <button className="btn btn-primary" type="button" style={{ backgroundColor: "#242424" }} onClick={() => setShowPassword(!showPassword)}>
                                    {showPassword ? <EyeSlashFill color="white" size={20} /> : <Eye color="white" size={20}></Eye>}
                                </button>

                            </InputGroup>

                        </Form.Group>
                    </Col>

                    <Col style={{ padding: "10px" }}>
                        <Form.Group className="mb-3" >
                            <Form.Label>Фамилия</Form.Label>
                            <InputGroup hasValidation  >
                                <Form.Control id="surname" name="surname" />
                            </InputGroup>

                            <Form.Label>Имя</Form.Label>
                            <InputGroup hasValidation>
                                <Form.Control id="name" name="name" />
                            </InputGroup>

                            <Form.Label>Отчество</Form.Label>
                            <InputGroup hasValidation >
                                <Form.Control id="patronymic" name="patronymic" />
                            </InputGroup>
                        </Form.Group>

                    </Col>
                    <Button className="btn btn-primary" variant="secondary" type="submit" id="submBtn" >Зарегестрироваться</Button>
                </Row>
            </Form>
            <Toast
                onClose={() => setShowErrorNotification(false)}
                show={showErrorNotification}
                delay={10000}
                autohide
                bg="danger"
                style={{
                    position: 'fixed',
                    bottom: 80,
                    right: 20,
                    minWidth: '200px',
                    zIndex: 1050

                }}
            >
                <Toast.Header>
                    <strong className="me-auto">Ошибка</strong>
                </Toast.Header>
                <Toast.Body className="text-white">{errorMessage}</Toast.Body>
            </Toast>
        </Container>
    </>)
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
