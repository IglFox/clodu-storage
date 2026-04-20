<template>
  <canvas ref="chartCanvas" width="400" height="300"></canvas>
</template>

<script setup>
import { ref, onMounted, watch } from "vue";
import Chart from "chart.js/auto";

const props = defineProps(["typeStats"]);
const chartCanvas = ref(null);
let chartInstance = null;

const renderChart = () => {
  if (!chartCanvas.value) return;
  if (chartInstance) chartInstance.destroy();
  const ctx = chartCanvas.value.getContext("2d");
  const labels = ["Документы", "Изображения", "Медиа", "Архивы", "Прочее"];
  const data = [
    props.typeStats.documents,
    props.typeStats.images,
    props.typeStats.media,
    props.typeStats.archives,
    props.typeStats.other,
  ];
  const textColor = getComputedStyle(document.documentElement)
    .getPropertyValue("--text-muted")
    .trim();
  chartInstance = new Chart(ctx, {
    type: "doughnut",
    data: {
      labels,
      datasets: [
        {
          data,
          backgroundColor: [
            "#3b82f6",
            "#22c55e",
            "#a855f7",
            "#f59e0b",
            "#8b93a7",
          ],
          borderWidth: 0,
        },
      ],
    },
    options: {
      responsive: true,
      maintainAspectRatio: true,
      plugins: {
        legend: {
          position: "bottom",
          labels: { color: textColor || "#8b93a7" },
        },
      },
    },
  });
};

onMounted(renderChart);
watch(() => props.typeStats, renderChart, { deep: true });
</script>
