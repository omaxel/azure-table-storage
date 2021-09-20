const $addForm = $('#form-add');

$addForm.submit(async e => {
    e.preventDefault();

    const data = new FormData($addForm[0]);

    try {
        const response = await fetch('/api/todos', {
            method: 'POST',
            body: data
        });

        if (!response.ok) {
            throw new Error(`Server didn't return 200.`);
        }

        location = '/';
    } catch (e) {
        alert('Error! Open DevTools for more details.');
        console.error('An error occured:', e);
    }
});