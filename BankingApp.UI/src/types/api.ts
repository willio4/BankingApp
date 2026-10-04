export interface AuthResponse {
    accessToken: string,
    refreshToken: string,
    customer: CustomerProfile,
}

export interface CustomerProfile {
    id: string;
    userId: string;
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    accounts: Account[];
    dateOfBirth: string,
    customerStatus: string,
    
}

export interface Account {
    id: string;
    accountNumber: string;
    balance: number;
    currency: string;
    type: string;
    accountStatus: string;
}