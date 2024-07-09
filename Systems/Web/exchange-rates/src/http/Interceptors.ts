import axios from 'axios';
import AuthResponse from '@models/AuthResponse';
import defaultUri from '@utils/defaultRoute';

export const API_AUTH_URL = `${defaultUri}`;

const $api = axios.create({
    baseURL: API_AUTH_URL,
    withCredentials: true
})

$api.interceptors.request.use((config) => {
    config.headers.Authorization = `Bearer ${localStorage.getItem('accessToken')}`
    return config;
})

$api.interceptors.response.use((config) => {
    return config;
}, async (error) => {
    const originalRequest = error.config;
    if (error.response.status == 401 && error.config && !error.config._isRetry) {
        originalRequest._isRetry = true;
        try {
            console.log(localStorage.getItem("перед обновлением"))
            const response = await axios.post<AuthResponse>(`${API_AUTH_URL}/auth/refresh?refreshToken=${localStorage.getItem("refreshToken")}`);
            console.log(response);
            localStorage.setItem('accessToken', response.data.data.accessToken);
            localStorage.setItem('refreshToken', response.data.data.refreshToken);
            console.log("Обновление токенов");
            return $api.request(originalRequest);
        } catch (e) {
            console.log('Not auth')
            localStorage.removeItem('accessToken');
            localStorage.removeItem('refreshToken');
            localStorage.removeItem('id');
            localStorage.removeItem('name');
        }
    }
    throw error;
})

export default $api;