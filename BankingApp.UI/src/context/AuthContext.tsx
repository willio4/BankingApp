import React, { createContext, useContext, useState, useEffect, type ReactNode } from 'react';
import { type AuthResponse } from '../types/api';
import { api } from '../api/axiosClient';

interface AuthContextType {
    user: AuthResponse | null;
    isAuthenticated: boolean;
    isLoading: boolean;
    login: (data: AuthResponse, refreshToken: string) => void;
    logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: ReactNode }> = ({ children }) => {
    const [ user, setUser ] = useState<AuthResponse | null>(null);
    const [ isLoading, setIsLoading ] = useState<boolean>(true);

    useEffect(() => {
        const accessToken = localStorage.getItem("accessToken");
        const refreshToken = localStorage.getItem("refreshToken");
        const storedCustomer = localStorage.getItem("user_profile");

        if (accessToken && refreshToken && storedCustomer) {
            try {
                // eslint-disable-next-line react-hooks/set-state-in-effect
                setUser({
                  accessToken,
                  refreshToken,
                  customer: JSON.parse(storedCustomer),
                });
            } catch {
                localStorage.clear();
            }
        }
        setIsLoading(false);
    }, []);

    const login = (authData: AuthResponse) => {
        localStorage.setItem('accessToken', authData.accessToken);
        localStorage.setItem('refreshToken', authData.refreshToken);
        localStorage.setItem('user_profile', JSON.stringify(authData.customer));
        setUser(authData);
    }

    const logout = async () => {
        try {
            const refreshToken = localStorage.getItem('refreshToken');
            if(refreshToken) {
                await api.post('auth/logout', {
                    RefreshToken: refreshToken
                });
            }
        } catch {
            // intentionally left blank
        } finally {
            localStorage.clear();
            window.location.href = '/login'
        }
    };

    return (
        <AuthContext.Provider value={{ user, isAuthenticated: !!user, isLoading, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

// eslint-disable-next-line react-refresh/only-export-components
export const useAuth = () => {
    const context = useContext(AuthContext);
    if(!context) {
        throw new Error('useAuth must be used within an AuthProvider');
    }
    return context;
}