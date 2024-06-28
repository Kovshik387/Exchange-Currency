import { Container, Row, Col } from 'react-bootstrap';

export default function Footer() {
    return (
        <footer style={FooterStyle} className="p-3 fixed-bottom">
            <Container>
                <Row>
                    <Col className="text-center">
                        <p>© 2024 DSR Company. All Rights Reserved.</p>
                    </Col>
                </Row>
            </Container>
        </footer>
    );
};

const FooterStyle: React.CSSProperties = {
    backgroundColor : "#dadada",
    padding: '10px 0',
    height: "70px",
    boxShadow: '0 4px 8px rgba(0, 0, 0, 0.2)',
}