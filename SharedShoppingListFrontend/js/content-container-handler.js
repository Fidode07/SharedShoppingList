let activeContainer, containers;

function getActiveContainer(containers) {
    return Array.from(containers).find(container => container.classList.contains('active'));
}

function changeContainer(targetContainer) {
    const newContainer = document.getElementById(targetContainer + '-container');
    if (!newContainer) {
        return;
    }
    activeContainer.classList.remove('active');
    activeContainer = newContainer;
    activeContainer.classList.add('active');
}

function initContainerData() {
    containers = document.getElementsByClassName('content-container');
    activeContainer = getActiveContainer(containers);
}

// Init data when DOM is loaded
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initContainerData);
} else {
    initContainerData();
}
