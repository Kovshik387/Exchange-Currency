import React from "react";
import { Button, Container, Form, InputGroup } from "react-bootstrap";
import { EyeSlashFill, Eye } from "react-bootstrap-icons";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import { AppDispatch } from "@stores/store";
import { setAuth, signIn } from "./../store/account.Slice";

export default function SignInPage() {
    const dispatch = useDispatch<AppDispatch>();
    const navigate = useNavigate();

    const [showPassword, setShowPassword] = React.useState(false);
    const [errorEmail, setErrorEmail] = React.useState("");
    const [errorPassword, setErrorPassword] = React.useState("");
    const [email, setEmail] = React.useState("");
    const [password, setPassword] = React.useState("");

    const handleSubmit = async (event: React.SyntheticEvent<HTMLFormElement>) => {
        event.preventDefault();
        event.stopPropagation();

        const account = { email: email, password: password };
        const resultAction = await dispatch(signIn(account));
        if (signIn.rejected.match(resultAction)) {
            const errorMessage = resultAction.payload;
            console.log(errorMessage);
            if (errorMessage?.match("password")) {
                setErrorPassword(errorMessage);
            } else {
                setErrorEmail(errorMessage!);
            }
        } else if (signIn.fulfilled.match(resultAction)) {
            const data = resultAction.payload;

            if (data.data !== null) {
                localStorage.setItem("id", data.data.id!.toString());
                localStorage.setItem("accessToken", data.data.accessToken!);
                localStorage.setItem("refreshToken", data.data.refreshToken!);
                localStorage.setItem("name", data.data.name);
                dispatch(setAuth(data.data.name));
                navigate('/');
            }
            const errorMessage = data.errorMessage;
            if (data.errorMessage?.match("password")) {
                setErrorPassword(errorMessage);
            } else {
                setErrorEmail(errorMessage!);
            }
        }
    }

    return (
        <Container style={{ ...ContainerStyle, fontSize: "20px", marginTop: "250px", maxWidth: "400px", height: "100%" }}>
            <h3>Авторизация</h3>
            <Form onSubmit={handleSubmit}>
                <Form.Group className="mb-3" controlId="email">
                    <Form.Label>Почта</Form.Label>
                    <InputGroup hasValidation>
                        <Form.Control
                            isInvalid={errorEmail !== ""}
                            type="email"
                            name="email"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            placeholder="name@example.com"
                            required
                        />
                        <Form.Control.Feedback type="invalid">
                            {errorEmail}
                        </Form.Control.Feedback>
                    </InputGroup>
                </Form.Group>
                <Form.Group className="mb-3" id="userPassword">
                    <Form.Label>Пароль</Form.Label>
                    <InputGroup hasValidation>
                        <Form.Control
                            isInvalid={errorPassword !== ""}
                            type={showPassword ? "text" : "password"}
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            name="password"
                            id="passwordVisible"
                            placeholder="Password"
                        />
                        <Button
                            style={{ backgroundColor: "#242424" }}
                            variant="contained"
                            onClick={() => setShowPassword(!showPassword)}
                        >
                            {showPassword ? <EyeSlashFill color="white" size={20} /> : <Eye color="white" size={20} />}
                        </Button>
                        <Form.Control.Feedback type="invalid">
                            {errorPassword}
                        </Form.Control.Feedback>
                    </InputGroup>
                    {/* <Form.Label>
                        <a className="justify-content-end" href="/ForgotPassword">Забыли пароль?</a>
                    </Form.Label> */}
                </Form.Group>
                <Button className="btn btn-primary" variant="secondary" type="submit">Войти</Button>
            </Form>
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
