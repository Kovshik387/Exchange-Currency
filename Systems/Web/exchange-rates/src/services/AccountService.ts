import $api from "@http/Interceptors";
import { AxiosResponse } from 'axios';
import AccountResponse from "@models/AccountResponse";

export default class AccountService {
    static async getProflile(id: string): Promise<AxiosResponse<AccountResponse>> {
        return $api.get<AccountResponse>(`account/${id}`);
    }
    static async changeForward(id: string): Promise<AxiosResponse<AccountResponse>> {
        return $api.patch<AccountResponse>(`account/change-forward?id=${id}`);
    }
    static async addVolute(id: string, name: string, volute: string): Promise<AxiosResponse<AccountResponse>> {
        return $api.post<AccountResponse>(`account/add-volute?id=${id}`, { volute: volute, name: name });
    }
    static async deleteVolute(id: string, name: string, volute: string): Promise<AxiosResponse<AccountResponse>> {
        return $api.delete<AccountResponse>(`account/delete-volute?id=${id}`, {
            data: {
                volute: volute, name: name
            }
        });
    }
}