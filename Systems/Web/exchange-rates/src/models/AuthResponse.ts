export interface AuthDetails {
    id: string;
    accessToken: string;
    refreshToken: string;
    name: string;
}

export default interface AuthResponse {
    data: AuthDetails;
    errorMessage: string;
};