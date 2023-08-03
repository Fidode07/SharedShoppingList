function handleContainer() {
    let containers = document.getElementsByClassName('content-container');
}

if(document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', handleContainer);
} else {
    handleContainer();
}
