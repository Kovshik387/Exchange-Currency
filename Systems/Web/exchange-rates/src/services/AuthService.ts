import $api from "@http/Interceptors";
import {AxiosResponse} from 'axios';
import AuthResponse from "@models/AuthResponse";
import SignUpModel from "@models/AuthSignUp";

export default class AuthService {
    static async signIn(email: string,password: string): Promise<AxiosResponse<AuthResponse>> {
        return $api.post<AuthResponse>("auth/signIn",{email,password})
    }
    static async registration(model: SignUpModel): Promise<AxiosResponse<AuthResponse>> {
        return $api.post<AuthResponse>('auth/signUp', model)
    }

    static async logout(refresh: string): Promise<AxiosResponse<AuthResponse>> {
        return $api.delete<AuthResponse>(`auth/logout?refreshToken=${refresh}`)
    }
}