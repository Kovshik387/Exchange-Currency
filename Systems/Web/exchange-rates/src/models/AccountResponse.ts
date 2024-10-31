export default interface AccountResponse {
    data: Account;
    errorMessage: string;
};

export interface Account {
    id: string;
    name: string;
    surname: string;
    patronymic: string;
    email: string;
    accept: boolean;
    url: string;
    favorites: Favorite[];
}

export interface Favorite {
    volute: string;
    name: string;
}