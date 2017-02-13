<template>
    <h1 class="message">{{ msg }}</h1>
</template>

<script>
export default {
  data () {
    return {
      msg: 'Hello World!  Using vue-loader.'
    }
  }
}
</script>

<style>
    .message {
        color: blue;
    }
</style>