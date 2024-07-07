import { createAsyncThunk, createSlice, PayloadAction } from '@reduxjs/toolkit';
import AuthService from '@services/AuthService';
import AuthResponse from '@models/AuthResponse';
import SignUpModel from '@models/AuthSignUp';

export interface AccountState {
    username: string;
    isLoggedIn: boolean;
    isLoading: boolean;
    error: string | null;
}

const initialState: AccountState = {
    username: '',
    isLoggedIn: false,
    isLoading: false,
    error: null,
};

export const signIn = createAsyncThunk<AuthResponse, { email: string; password: string }, { rejectValue: string }>(
    'auth/signIn',
    async ({ email, password }) => {
        try {
            const response = await AuthService.signIn(email, password);
            if (response.data.errorMessage === "") {
                setCreditions(response.data);
                return response.data;
            }
            return response.data;

        } catch (e: any) {
            return (await AuthService.signIn(email, password)).data;
        }
    }
);

export const registration = createAsyncThunk<AuthResponse, { model: SignUpModel }, { rejectValue: string }>(
    'auth/registration',
    async ({ model }) => {
        try {
            const response = await AuthService.registration(model);
            setCreditions(response.data);
            return response.data;
        } catch (e: any) {
            return (await AuthService.registration(model)).data;
        }
    }
);

export const logout = createAsyncThunk<void, void, { rejectValue: string }>(
    'auth/logout',
    async (_, { rejectWithValue }) => {
        try {
            await AuthService.logout(localStorage.getItem("refreshToken")!);
            localStorage.removeItem('accessToken');
            localStorage.removeItem('refreshToken');
            localStorage.removeItem('name');
            localStorage.removeItem('id');
        } catch (e: any) {
            return rejectWithValue(e.response?.data?.message);
        }
    }
);

const accountSlice = createSlice({
    name: 'account',
    initialState,
    reducers: {
        setAuth: (state, action: PayloadAction<string>) => {
            state.username = action.payload;
            state.isLoggedIn = true;
        },
        setUser(state, action: PayloadAction<string>) {
            state.username = action.payload;
        },
        setLoading(state, action: PayloadAction<boolean>) {
            state.isLoading = action.payload;
        },
        setError(state, action: PayloadAction<string | null>) {
            state.error = action.payload;
        },
        setLoggout(state, action: PayloadAction<boolean>) {
            state.isLoggedIn = action.payload;
        }
    },
});

export const { setAuth, setLoggout } = accountSlice.actions;

export default accountSlice.reducer;


function setCreditions(response: AuthResponse) {
    localStorage.setItem('accessToken', response.data.accessToken);
    localStorage.setItem('refreshToken', response.data.refreshToken);
    localStorage.setItem('name', response.data.name);
    localStorage.setItem('id', response.data.id);
}

