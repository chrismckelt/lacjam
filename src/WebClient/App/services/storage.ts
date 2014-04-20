/**
 * Services that persists and retrieves TODOs from localStorage
 */

class Storage {
    static STORAGE_ID = 'smsf-minder';

    // dependencies would be injected here
    constructor() {

    }

    get():Todo[] {
        return JSON.parse(localStorage.getItem(Storage.STORAGE_ID) || '[]');
    }

    put(todos:Todo[]) {
        localStorage.setItem(Storage.STORAGE_ID, JSON.stringify(todos));
    }
}

todomvc.service('todoStorage', TodoStorage)
