import { Account, Favorite } from "@models/AccountResponse";
import { CurrencyMarket, Volute } from "@models/Market";
import AccountService from "@services/AccountService";
import Routes from "@utils/routers";
import axios from "axios";
import { format } from "date-fns";
import { useEffect, useState } from "react";
import { Badge, Button, CloseButton, Col, Container, Form, InputGroup, Modal, Row, Spinner, Stack } from "react-bootstrap";
import { Typeahead } from "react-bootstrap-typeahead";
import { useParams } from "react-router-dom";

export default function ProfilePage() {
    const [account, setAccount] = useState<Account>();
    const [isLoading, setIsLoading] = useState(false);
    const [showModal, setShowModal] = useState(false);
    const [selectedFile, setSelectedFile] = useState<File | null>(null);
    const [selectedOption, setSelectedOption] = useState<boolean>(false);
    const searchParams = useParams();
    const [currencyData, setCurrencyData] = useState<CurrencyMarket | null>(null);
    const [newCurrency, setNewCurrency] = useState<Favorite | null>(null);
    const [edit, setEdit] = useState<boolean>(false);
    const id = searchParams["id"];
    const fetchVoluteData = async () => {
        const response = await axios.get(Routes.ExchangeRates, {
            params: { date: format(new Date(), 'dd.MM.yyyy') },
        });
        setCurrencyData(response.data);
    };

    const addVoluteData = async () => {
        if (newCurrency) {
            await AccountService.addVolute(id!, newCurrency.name, newCurrency.volute);
        }
    }

    const removeVoluteData = async (currency: Favorite) => {
        await AccountService.deleteVolute(id!, currency.name, currency.volute);
    }

    const handleAddCurrency = () => {
        if (newCurrency && !account?.favorites.find(fav => fav.name === newCurrency.name)) {
            const updatedFavorites = [...(account?.favorites || []), newCurrency];
            setAccount({ ...account!, favorites: updatedFavorites });
            addVoluteData();
            setNewCurrency(null);
        }
    };

    const handleRemoveCurrency = (currency: Favorite) => {
        if (account) {
            const updatedFavorites = account.favorites.filter(fav => fav.name !== currency.name);
            setAccount({ ...account, favorites: updatedFavorites });
            removeVoluteData(currency);
        }
    };

    const fetchAccount = async () => {
        setIsLoading(true);
        try {
            const id = searchParams["id"];
            if (!id) {
                console.log("Error id unknown");
                return;
            }
            const response = await AccountService.getProflile(id!);
            fetchVoluteData();
            setAccount(response.data.data);
            console.log(response.data.data)
            setSelectedOption(response.data.data.accept);
        } catch (error) {
            console.error('Error fetching account data:', error);
        } finally {
            setIsLoading(false);
        }
    };

    const handleRadioChange = async (event: React.ChangeEvent<HTMLInputElement>) => {
        const newOption = event.target.checked;
        setSelectedOption(newOption);
        try {
            await AccountService.changeForward(id!);
        } catch (error) {
            console.error('Error updating preference:', error);
        }
    };

    const handleCloseModal = () => setShowModal(false);
    const handleShowModal = () => setShowModal(true);

    const handleFileChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        if (event.target.files && event.target.files[0]) {
            setSelectedFile(event.target.files[0]);
        }
    };

    const handleUploadPhoto = async () => {
        if (selectedFile) {
            var response = await AccountService.uploadImage(selectedFile,id!)
            console.log(response)
            setAccount({ ...account!, url: response.data.data });
            handleCloseModal();
        }
    };

    const handleDeletePhoto = async () => {
        if (account?.url != ""){

        }
        handleCloseModal();
    }

    useEffect(() => {
        fetchAccount();
        if (id === localStorage.getItem("id")) {
            setEdit(true);
        }
    }, []);

    const getCurrencyQuote = (currencyName: string) => {
        const quote = currencyData?.volute.find(volute => volute.name === currencyName)?.value.toFixed(2);
        return quote ? ` - ${quote}` : '';
    };

    return (
        <>

            <Container style={{ ...ContainerStyle, fontSize: "20px", maxWidth: "1000px" }}>
                {isLoading ? (
                    <div style={{ textAlign: 'center' }}>
                        <Spinner animation="border" role="status">
                        </Spinner>
                    </div>
                ) : (
                    <>
                        <Row>
                            <Col sm="auto">
                                <img onClick={handleShowModal} style={avatarStyle} src={account?.url == "" ? "/Default.png" : account?.url}></img>
                            </Col>
                            <Col>
                                <Row>
                                    <Col sm="auto">
                                        <h3>{account?.name}</h3>
                                    </Col>
                                    <Col sm="auto">
                                        <h3>{account?.surname}</h3>
                                    </Col>
                                    <Col sm="auto">
                                        <h3>{account?.patronymic}</h3>
                                    </Col>
                                    <Col sm="auto">
                                    </Col>
                                </Row>
                                <Form hidden={!edit}>
                                    <Form.Check
                                        type="switch"
                                        label="Получать рассылку валют на почту"
                                        name="preference"
                                        checked={selectedOption}
                                        onChange={handleRadioChange}
                                        className="text-secondary"
                                        readOnly={!edit}
                                        style={{ color: '#6c757d' }}
                                    />
                                </Form>
                            </Col>
                        </Row>
                        <hr />
                        <Row>
                            <Col xs={12}>
                                <InputGroup hidden={!edit}>
                                    <Typeahead
                                        id="currency-selector"
                                        labelKey="name"
                                        options={currencyData ? currencyData.volute : []}
                                        placeholder="Добавить валюту"
                                        selected={newCurrency ? [newCurrency] : []}
                                        onChange={(selected) => {
                                            const selectedVolute = selected[0] as Volute;
                                            if (selectedVolute) {
                                                setNewCurrency({ name: selectedVolute.name, volute: selectedVolute.id });
                                            } else {
                                                setNewCurrency(null);
                                            }
                                        }}
                                    />
                                    <Button variant="outline-secondary" onClick={handleAddCurrency}>Добавить</Button>
                                </InputGroup>
                            </Col>
                        </Row>
                        <br />
                        <Row className="mb-4">
                            <Col xs={12}>
                                {account?.favorites.map((currency, index) => (
                                    <Badge key={index} bg="secondary" className="mb-2 me-2">
                                        <Stack direction="horizontal" className='py-2' gap={2}>
                                            {currency.name}{getCurrencyQuote(currency.name)}
                                            <Stack direction="horizontal" gap={2} hidden={!edit}>
                                                <CloseButton onClick={() => handleRemoveCurrency(currency)} />
                                            </Stack>
                                        </Stack>
                                    </Badge>
                                ))}
                            </Col>
                        </Row>
                    </>
                )}
                <Row></Row>
            </Container>
            <Modal show={showModal} onHide={handleCloseModal}>
                <Modal.Header closeButton>
                    <Modal.Title>Загрузить новое фото</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <Form.Group controlId="formFile" className="mb-3">
                        <Form.Label>Выберите изображение</Form.Label>
                        <Form.Control type="file" onChange={handleFileChange} />
                    </Form.Group>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleCloseModal}>
                        Закрыть
                    </Button>
                    <Button variant="danger" onClick={handleCloseModal}>
                        Удалить
                    </Button>
                    <Button variant="primary" onClick={handleUploadPhoto}>
                        Загрузить
                    </Button>
                </Modal.Footer>
            </Modal>
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

const avatarStyle: React.CSSProperties = {
    width: '60px',
    height: '60px',
    borderRadius: '50%',
    objectFit: 'cover',
    marginTop: '5px',
};
