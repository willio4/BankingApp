export interface AuthResponse {
    token: string;
    userId: string;
    customerId: string;
    email: string;
    firstName: string;
    lastName: string;
}

export interface CustomerProfile {
    id: string;
    userId: string;
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
    accounts: Account[];
}

export interface Account {
    id: string;
    accountNumber: string;
    balance: number;
    currency: string;
    accountType: string;
}