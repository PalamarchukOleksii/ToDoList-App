document.addEventListener('DOMContentLoaded', () => {
    const registerForm = document.getElementById('register-form');
    const loginForm = document.getElementById('login-form');
    const logoutBtn = document.getElementById('logout-btn');
    const createForm = document.getElementById('creating-form');
    const editingForm = document.getElementById('editing-form');
    const tasksContainer = document.getElementById('tasks');
    const showCreateFormBtn = document.getElementById('show-create-form-btn');
    const cancelCreateBtn = document.getElementById('cancel-create-btn');
    const cancelEditBtn = document.getElementById('cancel-edit-btn');
    const createSection = document.getElementById('create');
    const editSection = document.getElementById('edit');
    const taskListSection = document.getElementById('task-list');
    const buttonsSection = document.getElementById('buttons');

    const apiBaseUrl = 'http://localhost:5055/api';

    function clearToken() {
        localStorage.removeItem('token');
    }

    function showMainSections() {
        buttonsSection.classList.remove('hidden');
        taskListSection.classList.remove('hidden');
        createSection.classList.add('hidden');
        editSection.classList.add('hidden');
    }

    if (logoutBtn) {
        logoutBtn.addEventListener('click', () => {
            clearToken();
            window.location.href = 'index.html';
        });
    }

    if (showCreateFormBtn) {
        showCreateFormBtn.addEventListener('click', () => {
            buttonsSection.classList.add('hidden');
            taskListSection.classList.add('hidden');
            createSection.classList.remove('hidden');
        });
    }

    if (cancelCreateBtn) {
        cancelCreateBtn.addEventListener('click', showMainSections);
    }

    if (cancelEditBtn) {
        cancelEditBtn.addEventListener('click', showMainSections);
    }

    if (createForm) {
        createForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const formData = new FormData(createForm);
            const data = {
                shortInfo: formData.get('short-info'),
                fullInfo: formData.get('full-info'),
                endDateTime: formData.get('end-datetime')
            };

            try {
                const response = await fetch(`${apiBaseUrl}/usertask`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + localStorage.getItem('token')
                    },
                    body: JSON.stringify(data)
                });

                if (response.ok) {
                    fetchTasks();
                    createForm.reset();
                    showMainSections();
                } else {
                    const errorData = await response.json();
                    alert(`Creating task failed: ${errorData.message || response.statusText}`);
                }
            } catch (error) {
                console.error('Error during creating task', error);
                alert('Creating failed: An error occurred');
            }
        });
    }

    if (registerForm) {
        registerForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const formData = new FormData(registerForm);
            const data = {
                username: formData.get('username'),
                email: formData.get('email'),
                password: formData.get('password')
            };

            try {
                const response = await fetch(`${apiBaseUrl}/user/register`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(data)
                });

                if (response.ok) {
                    const result = await response.json();
                    localStorage.setItem('token', result.token);
                    window.location.href = 'tasks.html';
                } else {
                    const errorData = await response.json();
                    alert(`Registration failed: ${errorData.message || response.statusText}`);
                }
            } catch (error) {
                console.error('Error during registration:', error);
                alert('Registration failed: An error occurred');
            }
        });
    }

    if (loginForm) {
        loginForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const formData = new FormData(loginForm);
            const data = {
                username: formData.get('username'),
                password: formData.get('password')
            };

            try {
                const response = await fetch(`${apiBaseUrl}/user/login`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(data)
                });

                if (response.ok) {
                    const result = await response.json();
                    localStorage.setItem('token', result.token);
                    window.location.href = 'tasks.html';
                } else {
                    const errorData = await response.json();
                    alert(`Login failed: ${errorData.message || response.statusText}`);
                }
            } catch (error) {
                console.error('Error during login:', error);
                alert('Login failed: An error occurred');
            }
        });
    }

    async function fetchTasks() {
        try {
            const response = await fetch(`${apiBaseUrl}/usertask`, {
                headers: {
                    'Authorization': 'Bearer ' + localStorage.getItem('token')
                }
            });

            if (response.ok) {
                const tasks = await response.json();
                tasksContainer.innerHTML = '';

                tasks.forEach(task => {
                    const taskDiv = document.createElement('div');
                    taskDiv.classList.add('task');
                    taskDiv.dataset.id = task.id;

                    const shortInfo = document.createElement('h3');
                    shortInfo.textContent = task.shortInfo;
                    taskDiv.appendChild(shortInfo);

                    const fullInfo = document.createElement('p');
                    fullInfo.textContent = task.fullInfo;
                    taskDiv.appendChild(fullInfo);

                    const startDateTime = document.createElement('p');
                    startDateTime.textContent = `Start Date: ${task.startDateTime}`;
                    taskDiv.appendChild(startDateTime);

                    const endDateTime = document.createElement('p');
                    endDateTime.textContent = `End Date: ${task.endDateTime}`;
                    taskDiv.appendChild(endDateTime);

                    const status = document.createElement('p');
                    status.textContent = `Status: ${task.isDone ? 'Done' : 'Not Done'}`;
                    taskDiv.appendChild(status);

                    const editButton = document.createElement('button');
                    editButton.textContent = 'Edit';
                    editButton.style.marginRight = '10px'; // Add margin between buttons
                    editButton.onclick = () => {
                        loadTaskIntoForm(task);
                        buttonsSection.classList.add('hidden');
                        taskListSection.classList.add('hidden');
                        createSection.classList.add('hidden');
                        editSection.classList.remove('hidden');
                    };
                    taskDiv.appendChild(editButton);

                    const deleteButton = document.createElement('button');
                    deleteButton.textContent = 'Delete';
                    deleteButton.onclick = () => deleteTask(task.id);
                    taskDiv.appendChild(deleteButton);

                    tasksContainer.appendChild(taskDiv);
                });
            } else {
                console.error('Error fetching tasks:', response.statusText);
            }
        } catch (error) {
            console.error('Error fetching tasks:', error);
        }
    }

    async function deleteTask(taskId) {
        try {
            const response = await fetch(`${apiBaseUrl}/usertask/${taskId}`, {
                method: 'DELETE',
                headers: {
                    'Authorization': 'Bearer ' + localStorage.getItem('token')
                }
            });

            if (response.ok) {
                document.querySelector(`#tasks .task[data-id="${taskId}"]`).remove();
            } else {
                console.error('Error deleting task:', response.statusText);
            }
        } catch (error) {
            console.error('Error deleting task:', error);
        }
    }

    function loadTaskIntoForm(task) {
        document.getElementById('edit-task-id').value = task.id;
        document.getElementById('edit-short-info').value = task.shortInfo;
        document.getElementById('edit-full-info').value = task.fullInfo;
        document.getElementById('edit-is-done').checked = task.isDone;
        document.getElementById('edit-start-datetime').value = task.startDateTime.split('T')[0];
        document.getElementById('edit-end-datetime').value = task.endDateTime.split('T')[0];
    }

    if (editingForm) {
        editingForm.addEventListener('submit', async (e) => {
            e.preventDefault();
            const taskId = document.getElementById('edit-task-id').value;
            const formData = new FormData(editingForm);
            const data = {
                shortInfo: formData.get('edit-short-info'),
                fullInfo: formData.get('edit-full-info'),
                isDone: formData.get('edit-is-done') === 'on',
                endDateTime: formData.get('edit-end-datetime')
            };

            try {
                const response = await fetch(`${apiBaseUrl}/usertask/${taskId}`, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + localStorage.getItem('token')
                    },
                    body: JSON.stringify(data)
                });

                if (response.ok) {
                    fetchTasks();
                    editingForm.reset();
                    showMainSections();
                } else {
                    const errorData = await response.json();
                    alert(`Editing task failed: ${errorData.message || response.statusText}`);
                }
            } catch (error) {
                console.error('Error during editing task', error);
                alert('Editing failed: An error occurred');
            }
        });
    }

    if (tasksContainer) {
        fetchTasks();
    }
});
