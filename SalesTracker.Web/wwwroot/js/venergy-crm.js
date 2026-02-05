let projects = JSON.parse(localStorage.getItem('veProjects_v12')) || [];
let leadChannels = JSON.parse(localStorage.getItem('veChannels_v12')) || ["Eigen leads", "Davy", "SMA", "SolarWatt", "Wienerberger"];
const cats = ["PV-BAT B2C", "PV-BAT B2B", "WeVolt", "Lightweight PV", "BESS", "Charge", "ALSB"];
const reasons = ["Prijs", "Concurrent", "Geannuleerd", "Uitgesteld", "Te laat"];

let catTargets = JSON.parse(localStorage.getItem('veCatTargets_v12')) || {};

let currentSort = { key: 'date', asc: true };
let myCharts = {};
let currentPage = 1;
const rowsPerPage = 20;

function parseNum(val) {
    if (val === null || val === undefined || val === '') return null;
    let n = val.toString().replace(',', '.').trim();
    let parsed = parseFloat(n);
    return isNaN(parsed) ? null : parsed;
}

function calculateDiff(s, e) {
    if (!s || !e) return null;
    return Math.floor((new Date(e) - new Date(s)) / (1000 * 60 * 60 * 24));
}

function saveHourlyRate() {
    const val = document.getElementById('hourlyRate').value;
    localStorage.setItem('veHourlyRate_v12', val);
    renderDashboard();
}

function showSection(id) {
    ['input', 'kanban', 'table', 'stats'].forEach(s => {
        document.getElementById(s + '-section').classList.add('hidden');
        document.getElementById('nav-' + s).classList.remove('tab-active');
    });
    document.getElementById(id + '-section').classList.remove('hidden');
    document.getElementById('nav-' + id).classList.add('tab-active');
    if (id === 'stats') renderDashboard();
    if (id === 'table') renderTable();
    if (id === 'kanban') renderKanban();
}

function initApp() {
    const catHtml = cats.map(c => `<option value="${c}">${c}</option>`).join('');
    document.getElementById('category').innerHTML = catHtml;
    document.getElementById('tableCatFilter').innerHTML = `<option value="all">Alle Categorieën</option>` + catHtml;
    document.getElementById('kanbanCat').innerHTML = `<option value="all">Alle Categorieën</option>` + catHtml;
    document.getElementById('statsCat').innerHTML = `<option value="all">Alle Categorieën</option>` + catHtml;
    document.getElementById('lostReason').innerHTML = reasons.map(r => `<option value="${r}">${r}</option>`).join('');

    const storedRate = localStorage.getItem('veHourlyRate_v12');
    if (storedRate) document.getElementById('hourlyRate').value = storedRate;

    updateChannelDropdowns();
    renderDashboard();
    renderTable();
}

function updateChannelDropdowns() {
    const select = document.getElementById('leadChannel');
    const filter = document.getElementById('tableChannelFilter');
    let options = leadChannels.map(c => `<option value="${c}">${c}</option>`).join('');
    select.innerHTML = options;
    filter.innerHTML = `<option value="all">Alle Kanalen</option>` + options;
}

function addNewChannel() {
    const newChan = prompt("Nieuw lead kanaal:");
    if (newChan && !leadChannels.includes(newChan)) {
        leadChannels.push(newChan);
        localStorage.setItem('veChannels_v12', JSON.stringify(leadChannels));
        updateChannelDropdowns();
        document.getElementById('leadChannel').value = newChan;
    }
}

function exportToCSV() {
    if (projects.length === 0) return alert("Geen data.");
    const headers = ["date", "customer", "clientType", "category", "leadChannel", "status", "amount", "purchase", "manualMargin", "hours", "cafcaMargin", "cafcaHours", "notes", "endDate", "lostReason", "finalInvoiceAmount"];
    const csvRows = [headers.join(',')];
    for (const p of projects) {
        const row = headers.map(header => `"${(p[header] || '').toString().replace(/"/g, '""')}"`);
        csvRows.push(row.join(','));
    }
    const blob = new Blob([csvRows.join('\n')], { type: 'text/csv;charset=utf-8;' });
    const a = document.createElement('a');
    a.href = URL.createObjectURL(blob);
    a.download = `VENERGY_DATA.csv`;
    a.click();
}

function backupSystem() {
    const data = {
        projects: JSON.parse(localStorage.getItem('veProjects_v12')),
        channels: JSON.parse(localStorage.getItem('veChannels_v12')),
        targets: JSON.parse(localStorage.getItem('veCatTargets_v12')),
        rate: localStorage.getItem('veHourlyRate_v12')
    };
    const blob = new Blob([JSON.stringify(data, null, 2)], { type: 'application/json' });
    const a = document.createElement('a');
    a.href = URL.createObjectURL(blob);
    a.download = `VENERGY_BACKUP_${new Date().toISOString().slice(0, 10)}.json`;
    a.click();
}

function restoreSystem(event) {
    const file = event.target.files[0];
    if (!file) return;
    const reader = new FileReader();
    reader.onload = function (e) {
        try {
            const data = JSON.parse(e.target.result);
            if (confirm('Dit overschrijft ALLE huidige data. Weet je het zeker?')) {
                if (data.projects) localStorage.setItem('veProjects_v12', JSON.stringify(data.projects));
                if (data.channels) localStorage.setItem('veChannels_v12', JSON.stringify(data.channels));
                if (data.targets) localStorage.setItem('veCatTargets_v12', JSON.stringify(data.targets));
                if (data.rate) localStorage.setItem('veHourlyRate_v12', data.rate);
                location.reload();
            }
        } catch (err) {
            alert('Fout bij inlezen backup bestand.');
        }
    };
    reader.readAsText(file);
}

function renderDashboard() {
    const y = parseInt(document.getElementById('statsYear').value);
    const m = document.getElementById('statsMonth').value;
    const catF = document.getElementById('statsCat').value;

    let filtered = projects.filter(p => {
        const filterDate = p.status === 'won' ? p.endDate : p.date;
        if (!filterDate) return false;
        const d = new Date(filterDate);
        const yearMatch = d.getFullYear() === y;
        const monthMatch = (m === "all") || (d.getMonth() === parseInt(m));
        const catMatch = (catF === "all") || (p.category === catF);
        return yearMatch && monthMatch && catMatch;
    });

    const won = filtered.filter(p => p.status === 'won');
    const pending = filtered.filter(p => p.status === 'pending');
    const lost = filtered.filter(p => p.status === 'lost');

    const totalWon = won.reduce((s, p) => s + (p.finalInvoiceAmount || p.amount || 0), 0);
    document.getElementById('kpi-won').innerText = `€ ${totalWon.toLocaleString('nl-NL')}`;

    const yearlyTargets = catTargets[y] || {};
    let yearlyTargetTotal = cats.reduce((s, c) => s + (yearlyTargets[c] || 0), 0);
    const targetDivisor = (m === 'all' ? 1 : 12);
    let activeTarget = (catF === 'all') ? yearlyTargetTotal / targetDivisor : (yearlyTargets[catF] || 0) / targetDivisor;

    document.getElementById('kpi-target-perc').innerText = (activeTarget > 0 ? Math.round((totalWon / activeTarget) * 100) + '%' : '0%') + ` / € ${yearlyTargetTotal.toLocaleString('nl-NL')}`;

    const wCafca = won.filter(p => p.cafcaMargin !== null && p.cafcaMargin !== 0);
    document.getElementById('kpi-margin').innerText = wCafca.length ? Math.round(wCafca.reduce((s, p) => s + (p.cafcaMargin || 0), 0) / wCafca.length) + '%' : '0%';
    document.getElementById('kpi-pipe').innerText = `€ ${pending.reduce((s, p) => s + (p.amount || 0), 0).toLocaleString('nl-NL')}`;

    const leads = won.map(p => calculateDiff(p.date, p.endDate)).filter(d => d !== null && d >= 0);
    document.getElementById('kpi-lead').innerText = `${leads.length ? Math.round(leads.reduce((a, b) => a + b, 0) / leads.length) : 0} Dgn`;

    updateCharts(won, pending, lost, filtered, targetDivisor, catF, yearlyTargets);
    renderFinancialLeakage(won);
}

function renderFinancialLeakage(wonProjects) {
    const leakageBody = document.getElementById('leakage-body');
    const successBody = document.getElementById('success-body');
    const hRate = parseNum(document.getElementById('hourlyRate').value) || 0;
    leakageBody.innerHTML = '';
    successBody.innerHTML = '';

    const analyzed = wonProjects.filter(p => p.cafcaMargin !== null);
    analyzed.forEach(p => {
        const baseAmount = p.amount || 0;
        const actualAmount = p.finalInvoiceAmount || baseAmount;
        const baseMarginPerc = p.manualMargin || 0;
        const actualMarginPerc = p.cafcaMargin || 0;
        const baseMarginEuro = baseAmount * (baseMarginPerc / 100);
        const actualMarginEuro = actualAmount * (actualMarginPerc / 100);
        const hoursDiff = Number(((p.cafcaHours || 0) - (p.hours || 0)).toFixed(2));
        const laborImpact = hoursDiff * hRate;
        const totalFinancialImpact = (actualMarginEuro - baseMarginEuro) - laborImpact;
        const marginDiffPerc = actualMarginPerc - baseMarginPerc;

        const row = `<tr class="border-b last:border-0">
                    <td class="py-3 font-bold text-blue-900">${p.customer}</td>
                    <td class="py-3 text-center ${marginDiffPerc < 0 ? 'text-red-500' : 'text-green-500'} font-medium">${marginDiffPerc > 0 ? '+' : ''}${marginDiffPerc.toFixed(1)}%</td>
                    <td class="py-3 text-center text-slate-600 font-bold">${baseMarginPerc}% / <span class="${marginDiffPerc < 0 ? 'text-red-500' : 'text-green-500'}">${actualMarginPerc}%</span></td>
                    <td class="py-3 text-center ${hoursDiff > 0 ? 'text-red-500' : 'text-green-500'} font-medium">${hoursDiff > 0 ? '+' : ''}${hoursDiff}u</td>
                    <td class="py-3 text-center text-slate-500 font-bold">${p.hours || 0}u / <span class="${hoursDiff > 0 ? 'text-red-500' : 'text-green-500'}">${p.cafcaHours || 0}u</span></td>
                    <td class="py-3 text-center italic text-slate-500">${laborImpact > 0 ? '-' : '+'} € ${Math.abs(Math.round(laborImpact)).toLocaleString('nl-NL')}</td>
                    <td class="py-3 text-right font-black ${totalFinancialImpact < 0 ? 'text-red-600' : 'text-green-600'}">${totalFinancialImpact > 0 ? '+' : '-'} € ${Math.abs(Math.round(totalFinancialImpact)).toLocaleString('nl-NL')}</td>
                </tr>`;

        if (totalFinancialImpact < -1) leakageBody.innerHTML += row;
        else if (totalFinancialImpact > 1) successBody.innerHTML += row;
    });
}

function updateCharts(won, pending, lost, filtered, targetDiv, currentCat, yearlyTargets) {
    const activeCats = currentCat === 'all' ? cats : [currentCat];
    const config = {
        p: { ctx: 'pipelineChart', type: 'bar', label: 'Pipeline €', data: activeCats.map(c => pending.filter(p => p.category === c).reduce((s, p) => s + (p.amount || 0), 0)), bg: '#3b82f6' },
        c: { ctx: 'catChart', type: 'bar', datasets: [{ label: 'Behaald', data: activeCats.map(c => won.filter(p => p.category === c).reduce((s, p) => s + (p.finalInvoiceAmount || p.amount || 0), 0)), backgroundColor: '#10b981' }, { label: 'Target', data: activeCats.map(c => (yearlyTargets[c] || 0) / targetDiv), backgroundColor: '#e2e8f0' }] },
        cv: { ctx: 'convChart', type: 'bar', label: 'Conversie Ratio %', data: activeCats.map(c => { const t = filtered.filter(p => p.category === c).length; return t > 0 ? Math.round((won.filter(p => p.category === c).length / t) * 100) : 0; }), bg: '#6366f1' },
        m: { ctx: 'marginChart', type: 'bar', label: 'Marge %', data: activeCats.map(c => { const cw = won.filter(p => p.category === c && p.cafcaMargin); return cw.length ? Math.round(cw.reduce((s, p) => s + (p.cafcaMargin || 0), 0) / cw.length) : 0; }), bg: '#f97316' },
        l: { ctx: 'leadChart', type: 'bar', label: 'Dagen', data: activeCats.map(c => { const cl = won.filter(p => p.category === c).map(p => calculateDiff(p.date, p.endDate)).filter(d => d !== null); return cl.length ? Math.round(cl.reduce((a, b) => a + b, 0) / cl.length) : 0; }), bg: '#8b5cf6', horizontal: true },
        s: { ctx: 'spiderChart', type: 'radar', datasets: [{ label: 'Reden Lost', data: reasons.map(r => lost.filter(p => p.lostReason === r).length), backgroundColor: 'rgba(239, 68, 68, 0.2)', borderColor: '#ef4444' }] }
    };
    for (let key in config) {
        if (myCharts[key]) myCharts[key].destroy();
        const cf = config[key];
        myCharts[key] = new Chart(document.getElementById(cf.ctx), { type: cf.type, data: { labels: key === 's' ? reasons : activeCats, datasets: cf.datasets || [{ label: cf.label, data: cf.data, backgroundColor: cf.bg }] }, options: { maintainAspectRatio: false, indexAxis: cf.horizontal ? 'y' : 'x' } });
    }
}

function toggleStatusFields() {
    const s = document.getElementById('status').value;
    document.getElementById('wonDateContainer').classList.toggle('hidden', s !== 'won');
    document.getElementById('lostReasonContainer').classList.toggle('hidden', s !== 'lost');
}

document.getElementById('projectForm').addEventListener('submit', (e) => {
    e.preventDefault();
    const idx = parseInt(document.getElementById('editIndex').value);
    const data = {
        date: document.getElementById('date').value, customer: document.getElementById('customer').value,
        clientType: document.getElementById('clientType').value, category: document.getElementById('category').value,
        leadChannel: document.getElementById('leadChannel').value, status: document.getElementById('status').value,
        amount: parseNum(document.getElementById('amount').value) || 0, purchase: parseNum(document.getElementById('purchase').value) || 0,
        manualMargin: parseNum(document.getElementById('manualMargin').value) || 0, hours: parseNum(document.getElementById('hours').value) || 0,
        cafcaMargin: parseNum(document.getElementById('cafcaMargin').value), cafcaHours: parseNum(document.getElementById('cafcaHours').value),
        finalInvoiceAmount: document.getElementById('hasFinalInvoice').checked ? parseNum(document.getElementById('finalInvoiceAmount').value) : null,
        notes: document.getElementById('notes').value, endDate: document.getElementById('endDate').value || '', lostReason: document.getElementById('lostReason').value || ''
    };
    if (idx === -1) projects.push(data); else projects[idx] = data;
    localStorage.setItem('veProjects_v12', JSON.stringify(projects));
    document.getElementById('projectForm').reset();
    document.getElementById('editIndex').value = -1;
    document.getElementById('finalInvoiceContainer').classList.add('hidden');
    toggleStatusFields();
    showSection('stats');
});

function editProject(i) {
    const p = projects[i]; document.getElementById('editIndex').value = i;
    document.getElementById('date').value = p.date; document.getElementById('customer').value = p.customer;
    document.getElementById('clientType').value = p.clientType || 'Nieuw';
    document.getElementById('category').value = p.category; document.getElementById('leadChannel').value = p.leadChannel || leadChannels[0];
    document.getElementById('status').value = p.status;
    document.getElementById('amount').value = (p.amount || 0).toString().replace('.', ',');
    document.getElementById('purchase').value = (p.purchase || 0).toString().replace('.', ',');
    document.getElementById('manualMargin').value = (p.manualMargin || 0).toString().replace('.', ',');
    document.getElementById('hours').value = (p.hours || 0).toString().replace('.', ',');
    document.getElementById('cafcaMargin').value = p.cafcaMargin ? p.cafcaMargin.toString().replace('.', ',') : '';
    document.getElementById('cafcaHours').value = p.cafcaHours ? p.cafcaHours.toString().replace('.', ',') : '';

    if (p.finalInvoiceAmount) {
        document.getElementById('hasFinalInvoice').checked = true;
        document.getElementById('finalInvoiceContainer').classList.remove('hidden');
        document.getElementById('finalInvoiceAmount').value = p.finalInvoiceAmount.toString().replace('.', ',');
    } else {
        document.getElementById('hasFinalInvoice').checked = false;
        document.getElementById('finalInvoiceContainer').classList.add('hidden');
        document.getElementById('finalInvoiceAmount').value = '';
    }
    document.getElementById('notes').value = p.notes || ''; document.getElementById('endDate').value = p.endDate || '';
    document.getElementById('lostReason').value = p.lostReason || '';
    toggleStatusFields();
    showSection('input');
}

function sortData(k) {
    currentSort.asc = (currentSort.key === k) ? !currentSort.asc : true;
    currentSort.key = k;
    projects.sort((a, b) => {
        let vA = a[k] === null || a[k] === undefined ? '' : a[k];
        let vB = b[k] === null || b[k] === undefined ? '' : b[k];
        if (typeof vA === 'number' && typeof vB === 'number') return currentSort.asc ? vA - vB : vB - vA;
        vA = vA.toString().toLowerCase(); vB = vB.toString().toLowerCase();
        if (vA < vB) return currentSort.asc ? -1 : 1;
        if (vA > vB) return currentSort.asc ? 1 : -1;
        return 0;
    });
    renderTable();
}

function renderTable() {
    const tbody = document.getElementById('projectTableBody'); tbody.innerHTML = '';
    const q = document.getElementById('tableSearch').value.toLowerCase();
    const yF = document.getElementById('tableYearFilter').value;
    const cF = document.getElementById('tableCatFilter').value;
    const chF = document.getElementById('tableChannelFilter').value;

    let filteredProjects = projects.filter((p) => {
        const pY = new Date(p.date).getFullYear().toString();
        return (yF === 'all' || pY === yF) && (cF === 'all' || p.category === cF) && (chF === 'all' || p.leadChannel === chF) &&
            (p.customer.toLowerCase().includes(q) || (p.notes && p.notes.toLowerCase().includes(q)));
    });

    const totalRows = filteredProjects.length;
    const totalPages = Math.ceil(totalRows / rowsPerPage);
    const start = (currentPage - 1) * rowsPerPage;
    const end = start + rowsPerPage;
    const paginatedItems = filteredProjects.slice(start, end);

    paginatedItems.forEach((p, index) => {
        const actualIndex = projects.indexOf(p);
        const dispAmount = p.finalInvoiceAmount || p.amount || 0;
        tbody.innerHTML += `<tr>
                    <td class="p-4">${p.date}</td>
                    <td class="p-4 font-black text-blue-900">${p.customer}</td>
                    <td class="p-4"><span class="cat-badge">${p.category}</span></td>
                    <td class="p-4"><span class="channel-style">${p.leadChannel || 'Eigen'}</span></td>
                    <td class="p-4 text-center"><span>${p.hours || 0}u</span></td>
                    <td class="p-4 text-center"><span class="margin-style">${p.manualMargin}%</span></td>
                    <td class="p-4 text-right"><span class="amount-style">€ ${dispAmount.toLocaleString('nl-NL')}</span></td>
                    <td class="p-4 text-center"><span class="px-2 py-1 rounded status-${p.status}">${p.status.toUpperCase()}</span></td>
                    <td class="p-4 text-center flex gap-2 justify-center"><button onclick="editProject(${actualIndex})">✏️</button><button onclick="deleteProject(${actualIndex})">🗑️</button></td>
                </tr>`;
    });

    document.getElementById('projectCount').innerText = `${start + 1} - ${Math.min(end, totalRows)} van ${totalRows}`;

    document.getElementById('pageInfo').innerText = `Pagina ${currentPage} van ${totalPages}`;
    document.getElementById('prevPage').style.display = currentPage > 1 ? 'block' : 'none';
    document.getElementById('nextPage').style.display = currentPage < totalPages ? 'block' : 'none';
}

function changePage(direction) {
    const q = document.getElementById('tableSearch').value.toLowerCase();
    const yF = document.getElementById('tableYearFilter').value;
    const cF = document.getElementById('tableCatFilter').value;
    const chF = document.getElementById('tableChannelFilter').value;

    let filteredProjects = projects.filter((p) => {
        const pY = new Date(p.date).getFullYear().toString();
        return (yF === 'all' || pY === yF) && (cF === 'all' || p.category === cF) && (chF === 'all' || p.leadChannel === chF) &&
            (p.customer.toLowerCase().includes(q) || (p.notes && p.notes.toLowerCase().includes(q)));
    });

    const totalPages = Math.ceil(filteredProjects.length / rowsPerPage);
    currentPage += direction;
    currentPage = Math.max(1, Math.min(currentPage, totalPages));
    renderTable();
}

function deleteProject(i) {
    if (confirm('Wissen?')) {
        projects.splice(i, 1);
        localStorage.setItem('veProjects_v12', JSON.stringify(projects));
        renderTable();
    }
}

function renderKanban() {
    const container = document.getElementById('kanbanContainer'); container.innerHTML = '';
    const y = document.getElementById('kanbanYear').value;
    const m = document.getElementById('kanbanMonth').value;
    const catF = document.getElementById('kanbanCat').value;
    ['pending', 'won', 'lost'].forEach(s => {
        const filtered = projects.filter(p => {
            const filterDate = s === 'won' ? p.endDate : p.date;
            if (!filterDate) return false;
            const d = new Date(filterDate);
            return p.status === s && d.getFullYear().toString() === y && (m === 'all' || d.getMonth().toString() === m) && (catF === 'all' || p.category === catF);
        });
        const totalCol = filtered.reduce((acc, p) => acc + (p.finalInvoiceAmount || p.amount || 0), 0);
        const colorClass = s === 'won' ? 'text-green-600' : s === 'lost' ? 'text-red-600' : 'text-yellow-600';
        let h = `<div class="bg-slate-100 p-4 rounded-2xl min-h-[400px] border-t-4 ${s === 'won' ? 'border-green-500' : s === 'lost' ? 'border-red-500' : 'border-yellow-500'}">
                            <div class="flex justify-between items-start mb-4"><h3 class="font-black uppercase text-[11px]">${s}</h3><span class="font-black text-[11px] ${colorClass}">€ ${totalCol.toLocaleString('nl-NL')}</span></div>`;
        filtered.forEach(p => {
            const pIndex = projects.indexOf(p);
            const dispAmount = p.finalInvoiceAmount || p.amount || 0;
            const hasCafcaData = p.cafcaMargin !== null && p.cafcaHours !== null;
            const cardBg = hasCafcaData ? 'bg-blue-50' : 'bg-white';
            const cardBorder = s === 'won' ? 'border-green-500' : s === 'lost' ? 'border-red-500' : 'border-yellow-500';
            h += `<div class="${cardBg} p-3 rounded-xl mb-3 shadow-sm border-l-4 cursor-pointer hover:opacity-80 ${cardBorder}" onclick="editProject(${pIndex})">
                            <p class="font-black text-blue-900 text-[12px] flex justify-between"><span>${p.customer}</span><span class="${colorClass}">€${dispAmount.toLocaleString('nl-NL')}</span></p>
                            <div class="text-[9px] text-slate-400 mt-1 uppercase font-bold">${p.category}</div></div>`;
        });
        container.innerHTML += h + `</div>`;
    });
}

function showTargetModal() {
    const y = document.getElementById('statsYear').value;
    const c = document.getElementById('targetInputs');
    const yearlyTargets = catTargets[y] || {};

    document.getElementById('targetYearLabel').innerText = `Instellen voor jaar: ${y}`;
    c.innerHTML = cats.map(cat => `<div class="flex justify-between items-center"><label class="m-0">${cat}</label><input type="text" id="t-${cat}" value="${yearlyTargets[cat] || 0}" class="border rounded p-1 w-24 text-right"></div>`).join('');
    document.getElementById('targetModal').classList.remove('hidden');
}

function saveTargets() {
    const y = document.getElementById('statsYear').value;
    if (!catTargets[y]) catTargets[y] = {};

    cats.forEach(cat => {
        catTargets[y][cat] = parseNum(document.getElementById(`t-${cat}`).value);
    });

    localStorage.setItem('veCatTargets_v12', JSON.stringify(catTargets));
    document.getElementById('targetModal').classList.add('hidden');
    renderDashboard();
}

window.onload = () => { initApp(); showSection('stats'); document.getElementById('date').valueAsDate = new Date(); };
