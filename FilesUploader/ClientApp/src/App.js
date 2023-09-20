import React, {useState} from 'react';
import './App.css';
import axios from 'axios';

const App = () => {
    const [inputText, setInputText] = useState('');
    const [selectedFile, setSelectedFile] = useState(null);

    const handleFileChange = (e) => {
        setSelectedFile(e.target.files[0]);
    };

    const handleInputChange = (e) => {
        setInputText(e.target.value);
    };

    const handleButtonClick = () => {
        if (selectedFile) {
            const emailRegex = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i;
            if (inputText.match(emailRegex)) {
                const formData = new FormData();
                formData.append('file', selectedFile);
                formData.append('email', inputText);

                axios.post('/api/filecontroller/upload', formData)
                    .then(response => {
                        console.log('Файл успішно відправлено на сервер', response.data);
                        setInputText('');
                        setSelectedFile(null);
                    })
                    .catch(error => {
                        console.error('Помилка відправлення файлу', error);
                    });
            } else {
                alert('Введено неправильну електронну пошту. Будь ласка, введіть коректну електронну пошту.');
            }
        } else {
            alert('Виберіть файл .docx для завантаження.');
        }
    };


        return (
            <div className="container">
                <h5>Select a file</h5>
                <input
                    type="file"
                    onChange={handleFileChange}
                    accept=".docx"
                />
                <h5>Enter your email</h5>

                <input
                    type="text"
                    value={inputText}
                    onChange={handleInputChange}
                    placeholder="Enter your email"
                />

                <button onClick={handleButtonClick} className="button">Натисніть мене</button>
            </div>
        );
    };

    export default App;