import { CurrencyMarket } from '@models/Market';
import Routes from '@utils/routers'
import { Container, Form, Spinner, Stack } from 'react-bootstrap';
import React, { useEffect, useState } from 'react';
import VoluteTable from '@components/Volute/VoluteTable';
import { addDays, format } from 'date-fns';
import axios from 'axios';

const ContainerStyle: React.CSSProperties = {
  padding: '20px',
  marginTop: "65px",
  marginBottom: "100px",
  overflowY: "auto",
  boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
  borderRadius: '8px',
  backgroundColor: '#fff',
  height: "85vh"
};

export default function MainContent() {
  const [selectedDate, setSelectedDate] = useState<Date | null>(new Date());
  const [currencyData, setCurrencyData] = useState<CurrencyMarket | null>();
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [filteredData, setFilteredData] = useState<CurrencyMarket | null>(null);

  const fetchCurrencyData = async (date: Date) => {
    setIsLoading(true);
    try {
      const response = await axios.get(Routes.ExchangeRates, {
        params: { date: format(date, 'dd.MM.yyyy') },
      });
      setCurrencyData(response.data);
      setFilteredData(response.data);
    } catch (error) {
      console.error('Error fetching currency data:', error);
    } finally {
      setIsLoading(false);
    }
  };

  const handleDateChange = (event: any) => {
    const date = event.target.value ? new Date(event.target.value) : null;
    setSelectedDate(date);
  };

  const handleSearchChange = (event: any) => {
    if (currencyData) {
      if (event.target.value.trim() === '') {
        setFilteredData(currencyData);
      } else {
        const filtered = {
          ...currencyData,
          volute: currencyData.volute.filter((volute) =>
            volute.name.toLowerCase().includes(event.target.value.toLowerCase())
          )
        };
        setFilteredData(filtered);
      }
    }
  };
  useEffect(() => {
    if (selectedDate) {
      fetchCurrencyData(selectedDate);
    }
  }, [selectedDate]);

  const maxDate = format(addDays(new Date(), 1), 'yyyy-MM-dd');

  return (
    <div>
      <Container style={ContainerStyle}>
        <h1>Официальные курсы валют на заданную дату, устанавливаемые ежедневно</h1>
        <Form style={{ marginBottom: "20px" }}>
          <Stack direction='horizontal' gap={3}>

            <Form.Group controlId="formDate">
              <Form.Label className="mr-2">Дата:</Form.Label>
              <Form.Control
                type='date'
                onChange={handleDateChange}
                value={selectedDate ? format(selectedDate, 'yyyy-MM-dd') : ''}
                max={maxDate}
                className="mr-2"
                style={{ width: "150px" }}
              />
            </Form.Group>
            <Form.Group controlId="formSearch">
              <Form.Label className="mr-2">Поиск:</Form.Label>
              <Form.Control
                type='search'
                placeholder="Введите валюту"
                onChange={handleSearchChange}
                className="mr-2"
                style={{ width: "160px" }}
              />
            </Form.Group>
          </Stack>
        </Form>
        {isLoading ? (
          <div style={{ textAlign: 'center' }}>
            <Spinner animation="border" role="status">
              <span className="sr-only"></span>
            </Spinner>
          </div>
        ) : (
          filteredData && <VoluteTable data={filteredData} />
        )}
      </Container>
    </div>
  );
}