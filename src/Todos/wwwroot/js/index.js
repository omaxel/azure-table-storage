const $todos = $('#todo-list');
const $btnLoadMore = $('#btn-load-more');
const $msgNoMorePages = $('#msg-no-more-pages');

let nextContinuationToken = $todos.data('continuation-token');

async function fetchNextPage(reload = false) {
    if (!nextContinuationToken && !reload) {
        console.warn('No other pages available');
        return;
    }

    let url = '/api/todos';

    if (reload) {
        nextContinuationToken = null;
    } else {
        url += '?continuationToken=' + encodeURIComponent(nextContinuationToken);
    }

    let response;

    try {
        response = await fetch(url);

        if (!response.ok) {
            throw new Error(`Server didn't return 200.`);
        }
    } catch (e) {
        alert('Error! Open DevTools for more details.');
        console.error('An error occured:', e);
    }

    const jsonResponse = await response.json();

    nextContinuationToken = jsonResponse.nextContinuationToken;

    const html = jsonResponse.values.map(x => `
<div class="todo">
    <div class="todo__content">
	    <span class="todo__title">${x.title}</span>
        <span class="todo__description">${x.description}</span>
    </div>
    <a class="btn btn-sm btn-warning mr-2" href="/todos/edit/${x.partitionKey}/${x.rowKey}">Edit</a>
    <button class="btn btn-sm btn-danger" onclick="deleteTodo('${x.partitionKey}', '${x.rowKey}')">Delete</button>
</div>`).join('');

    if (reload) {
        $todos.html(html);
    } else {
        $todos.append(html);
    }

    $btnLoadMore.attr('hidden', !nextContinuationToken);
    $msgNoMorePages.attr('hidden', !!nextContinuationToken);
}

async function deleteTodo(partitionKey, rowKey) {
    try {
        const response = await fetch(`/api/todos/${encodeURIComponent(partitionKey)}/${encodeURIComponent(rowKey)}`, {
            method: 'DELETE'
        });

        if (!response.ok) {
            throw new Error(`Server didn't return 200.`);
        }
    } catch (e) {
        alert('Error! Open DevTools for more details.');
        console.error('An error occured:', e);
    }

    await fetchNextPage(true);
}