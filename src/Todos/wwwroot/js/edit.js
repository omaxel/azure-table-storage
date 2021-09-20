const $editForm = $('#form-edit');

$editForm.submit(async e => {
    e.preventDefault();

    const data = new FormData($editForm[0]);

    const partitionKey = data.get('partitionKey');
    const rowKey = data.get('rowKey');

    try {
        const response = await fetch(`/api/todos/${partitionKey}/${rowKey}`, {
            method: 'PUT',
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