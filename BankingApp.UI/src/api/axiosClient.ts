import axios from 'axios';

const API_BASE_URL = "https://localhost:7123/api";

export const api = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json'
    }
})

api.interceptors.request.use((config) => {
    const token = localStorage.getItem('accessToken');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
}, (error) => {
    return Promise.reject(error)
})

api.interceptors.response.use(
    (response) => response,
    async (error) => {
        const originalRequest = error.config;

        if(error.response?.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true;

            try {
                const refreshToken = localStorage.getItem('refreshToken');
                if(!refreshToken) {
                    throw new Error('No refresh token available')
                }

                const refreshResponse = await axios.post(`${API_BASE_URL}/auth/refresh`, {
                    RefreshToken: refreshToken,
                });

                const newAccessToken = refreshResponse.data.accessToken;

                localStorage.setItem('accessToken', newAccessToken);

                originalRequest.headers.Authorization = `Bearer ${newAccessToken}`;
                return api(originalRequest);
            }   catch (refreshError) {
                localStorage.removeItem('accessToken');
                localStorage.removeItem('refreshToken');
                window.location.href = "/login";
                return Promise.reject(refreshError)
            }
        }

        return Promise.reject(error);
    }
);