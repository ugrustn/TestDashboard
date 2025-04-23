import axios from 'axios';

const API_BASE_URL = 'http://localhost:5000/api';

const api = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json'
    }
});

// Add token to requests if available
api.interceptors.request.use((config) => {
    const token = localStorage.getItem('token');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

// Auth API
export const login = (email: string, password: string) =>
    api.post('/auth/login', { email, password });

export const register = (email: string, password: string, firstName: string, lastName: string) =>
    api.post('/auth/register', { email, password, firstName, lastName });

// Tests API
export const getTests = () =>
    api.get('/tests');

export const getTest = (id: number) =>
    api.get(`/tests/${id}`);

export const createTest = (data: {
    title: string;
    description: string;
    timeLimit: number;
    questions: {
        text: string;
        points: number;
        answers: {
            text: string;
            isCorrect: boolean;
        }[];
    }[];
}) => api.post('/tests', data);

// Test Attempts API
export const startTestAttempt = (testId: number) =>
    api.post(`/tests/${testId}/attempt`);

export const getTestAttempt = (id: number) =>
    api.get(`/tests/attempts/${id}`);

export const submitTestAttempt = (id: number, answerIds: number[]) =>
    api.post(`/tests/attempts/${id}/submit`, { testId: id, answerIds });

export default api; 